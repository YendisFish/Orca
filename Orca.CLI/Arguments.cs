using Orca.CLI.Config;
using Orca.Lib.Modules;
using Orca.Lib.Logging;
using Orca.Lib;
using Orca.Lib.Messages;
using Orca.Lib.Sockets;
using YamlDotNet.Serialization;
using System.Net.Sockets;

namespace Orca.CLI;

public class Arguments
{
    public static async Task Handle(string[] args)
    {
        for(int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "test":
                {
                    OrcaSocket socket = new OrcaSocket("/var/orca/orca.sock");
                    await socket.Write(new OrcaMessage(MessageType.DATA));
                    break;
                }

                case "r":
                case "rollout":
                {
                    await Rollout(args[(i+1)..]);
                    break;
                }
            }
        }
    }

    public static async Task Rollout(string[] args)
    {
        PodConfig? conf = null;
        if (args.Length == 0)
        {
            //build current directory
            string yaml = await File.ReadAllTextAsync("./orca.yaml");
            
            Deserializer d = new Deserializer();
            conf = d.Deserialize<PodConfig>(yaml);
        }
        
        // else handle for multiple args
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-p":
                {
                    string path = args[++i];
                    string yaml = await File.ReadAllTextAsync(Path.Join(path, "orca.yaml"));
           
                    try {
                        Deserializer d = new Deserializer();
                        conf = d.Deserialize<PodConfig>(yaml);
                    } catch(Exception ex) {
                        Logger.Console.Error(ex.Message);
                    }
                    break;
                }
            } 
        }

        if (conf == null) throw new FileNotFoundException("Could not find config!");

        List<MemberRequest> reqs = new();
        foreach(var pod in conf.Pods!.Values)
        {
            reqs.Add(new MemberRequest(MemberRequestType.BUILD,
                        new MemberBuildData()
                        {
                            Image = pod.Target,
                            BuildTasks = new()
                        }));
        }

        using OrcaSocket sock = new OrcaSocket("/var/orca/orca.sock");
        await sock.Write(new BuildMessage(MessageType.BUILD) 
                {
                    Requests = reqs
                });
        sock.Flush();

        await Streaming.Streaming.IntakeStream(sock);
    }
}
