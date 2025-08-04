using Orca.Daemon;
using Orca.Lib.Engine;
using Orca.Lib.Exceptions;
using Orca.Lib.Logging;

await Init.CreateGroup();
await Init.ConfigureFiles();
await Init.ConfigurePerms();

OrcaEngine engine = new OrcaEngine("/var/orca/orca.sock");

engine.SocketHub.OnMessageRecieved = async (msg) =>
{
    Logger.Console.Info("Message recieved!");
};

await engine.Start();