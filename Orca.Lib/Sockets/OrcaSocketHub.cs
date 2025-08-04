using System.Diagnostics;
using System.IO.Pipes;
using System.Net.Mime;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json;
using Orca.Lib.Exceptions;
using Orca.Lib.Logging;
using Orca.Lib.Messages;
using Orca.Lib.Wrappers;

namespace Orca.Lib.Sockets;

public class OrcaSocketHub : IDisposable
{
    internal Socket listenSock { get; set; }
    internal string sockPath { get; init; }
    public Dictionary<Socket, CancellationTokenSource> Sockets { get; set; } = new();
    
    public OrcaSocketHub(string socketPath)
    {
        if (File.Exists(socketPath))
        {
            File.Delete(socketPath);
        }
        
        this.sockPath = socketPath;
    }

    public async Task Start(int queueLength = 10)
    {
        var endPoint = new UnixDomainSocketEndPoint(sockPath);
        listenSock = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Unspecified);
        listenSock.Bind(endPoint);
        listenSock.Listen(queueLength);

        Logger.Console.Info($"Bound to {sockPath.AsColor(StringColor.Yellow)}");
        await OrcaSocketHub.SetOrcaSocketPermissions();
        
        while (true)
        {
            Socket client = await listenSock.AcceptAsync();
            CancellationTokenSource cts = new CancellationTokenSource();
            Sockets.Add(client, cts);
            _ = HandleSocket(cts.Token, client);
        }
    }
    
    public async Task HandleSocket(CancellationToken ct, Socket socket)
    {
        using var stream = new NetworkStream(socket, ownsSocket: false);
        using var reader = new StreamReader(stream);
        using var writer = new StreamWriter(stream);
        
        while (!ct.IsCancellationRequested)
        {
            string? line = await reader.ReadLineAsync();
            if (line is null) continue;

            try
            {
                OrcaMessage msg = JsonConvert.DeserializeObject<OrcaMessage>(line) ??
                                  throw new BadMessageException($"Could not parse message: {line}");

                if (OnMessageRecieved is not null) await OnMessageRecieved(msg);

                switch (msg.Type)
                {
                    case MessageType.TEST:
                    {
                        await Images.Pull("nginx:latest");
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
    
    public void Dispose() => listenSock.Dispose();

    public static async Task SetOrcaSocketPermissions()
    {
        Logger.Console.Info("Setting socket permissions...");
        
        ProcessStartInfo pinf = new ProcessStartInfo()
        {
            FileName = "chown",
            Arguments = "root:orca /var/orca/orca.sock",
            RedirectStandardOutput = false,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        
        
        ProcessStartInfo pinf2 = new ProcessStartInfo()
        {
            FileName = "chmod",
            Arguments = "660 /var/orca/orca.sock",
            RedirectStandardOutput = false,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        
        using Process p1 =  Process.Start(pinf) ?? throw new ProcessFailureException("Failed to change orca socket permissions");
        await p1.WaitForExitAsync();
        
        using Process p2 =  Process.Start(pinf2) ?? throw new ProcessFailureException("Failed to change orca socket permissions");
        await p2.WaitForExitAsync();
    }
    
    public delegate Task OnMessage(OrcaMessage msg);
    public OnMessage OnMessageRecieved { get; set; }
}