using System.Net.Sockets;
using Newtonsoft.Json;
using Orca.Lib.Messages;

namespace Orca.Lib.Sockets;

public class OrcaSocket : IDisposable
{
    private Socket sock { get; set; }
    private StreamReader reader { get; set; }
    private StreamWriter writer { get; set; }

    public OrcaSocket(string path)
    {
        var endPoint = new UnixDomainSocketEndPoint(path);
        sock = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Unspecified);
        
        sock.Connect(endPoint);
        
        var stream = new NetworkStream(sock, ownsSocket: false);
        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);
        writer.AutoFlush = true;
    }

    public async Task<OrcaMessage?> Read()
    {
        string? line = await reader.ReadLineAsync();
        if (line is null) return null;
        
        return JsonConvert.DeserializeObject<OrcaMessage>(line);
    }

    public async Task Write(OrcaMessage message)
    {
        string json = JsonConvert.SerializeObject(message);
        await writer.WriteAsync(json);
    }

    public void Listen() => sock.Listen(10);

    public void Dispose()
    {
        sock?.Dispose();
        reader?.Dispose();
        writer?.Dispose();
    }
}