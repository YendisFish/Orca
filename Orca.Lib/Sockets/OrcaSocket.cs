using System.Net.Sockets;
using Newtonsoft.Json;
using Orca.Lib.Messages;

namespace Orca.Lib.Sockets;

public class OrcaSocket : IDisposable
{
    public Socket sock { get; set; }
    public StreamReader reader { get; set; }
    public StreamWriter writer { get; set; }

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
        
        return JsonConvert.DeserializeObject<OrcaMessage>(line, Globals.Settings);
    }

    public async Task<OrcaMessage?> Read(CancellationToken tok)
    {
        string? line = await reader.ReadLineAsync(tok);
        if (line is null) return null;
        
        return JsonConvert.DeserializeObject<OrcaMessage>(line, Globals.Settings);
    }

    public async Task Write(OrcaMessage message)
    {
        string json = JsonConvert.SerializeObject(message, Globals.Settings);
        await writer.WriteLineAsync(json);
    }

    public void Listen() => sock.Listen(10);

    public void Flush() => writer.Flush();

    public void Dispose()
    {
        sock?.Dispose();
        reader?.Dispose();
        writer?.Dispose();
    }
}
