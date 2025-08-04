using Orca.Lib.Logging;
using Orca.Lib.Sockets;

namespace Orca.Lib.Engine;

public class OrcaEngine
{
    public OrcaSocketHub SocketHub { get; set; }
    internal string SocketPath { get; init; }

    public OrcaEngine(string socketPath)
    {
        SocketPath = socketPath;
        SocketHub = new OrcaSocketHub(SocketPath);
    }
    
    public async Task Start()
    {
        Logger.Console.Info("Starting engine...");
        await SocketHub.Start();
    }
}