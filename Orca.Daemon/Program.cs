using Orca.Daemon;
using Orca.Lib.Engine;
using Orca.Lib.Exceptions;

await Init.CreateGroup();
await Init.ConfigureFiles();
await Init.ConfigurePerms();

OrcaEngine engine = new OrcaEngine("/var/orca/orca.sock");

engine.SocketHub.OnMessageRecieved = async (msg) =>
{
    Console.WriteLine("EVENT LISTENER: OnMessageRecieved");
};

await engine.Start();