using Orca.CLI.Config;
using Orca.Lib;
using Orca.Lib.Messages;
using Orca.Lib.Sockets;
using YamlDotNet.Serialization;

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
                    await socket.Write(new OrcaMessage(MessageType.TEST));
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
                    string yaml = await File.ReadAllTextAsync("./orca.yaml");
            
                    Deserializer d = new Deserializer();
                    conf = d.Deserialize<PodConfig>(yaml);
                    
                    break;
                }
            } 
        }

        if (conf == null) throw new FileNotFoundException("Could not find config!");
        
        var r = new RolloutInfo()
        {
            ConfigName = conf.Name,
            DeploymentPlan = new Dictionary<string, string>()
            {
                { "A1", "0\n1\n2\n3\n4\n5\n6" },
                { "A2", "0\n1\n2\n3\n4\n5\n6" }
            },
            Progress = "0"
        };

        Writer.StartRollout(r);

        for (int i = 0; i < 94; i++) {
            foreach (var kvp in r.DeploymentPlan)
            {
                r.DeploymentPlan[kvp.Key] = $"{i}\n{i + 1}\n{i + 2}\n{i + 3}\n{i + 4}\n{i + 5}\n{i + 6}";
            }
            r.Progress = $"{i}";
    
            Writer.WriteRollout(r);
            Task.Delay(100).Wait();
        }
    }
}