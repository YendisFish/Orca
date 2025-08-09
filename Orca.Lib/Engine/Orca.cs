using Orca.Lib.Logging;
using Orca.Lib.Sockets;
using Orca.Lib.Modules;

namespace Orca.Lib.Engine;

public class OrcaEngine
{
    public OrcaSocketHub SocketHub { get; set; }
    internal string SocketPath { get; init; }

    public MemberHub MemberHub { get; set; }

    public OrcaEngine(string socketPath)
    {
        MemberHub = new MemberHub();
        SocketPath = socketPath;
        SocketHub = new OrcaSocketHub(SocketPath, this);
    }
    
    public async Task Start()
    {
        Logger.Console.Info("Starting engine...");

        await MemberHub.Start();
        await SocketHub.Start();
    }
}
