namespace Orca.Lib;

public class Writer
{
    public static void WriteLine()
    {
        
    }

    public static void StartRollout(RolloutInfo info)
    {
        int len = 0;
        Console.WriteLine($"{"Rolling out config".AsBold().AsColor(StringColor.Blue)} {info.ConfigName.AsColor(StringColor.Yellow)}");
        Console.WriteLine($"{"Deployment plan:".AsBold().AsColor(StringColor.Blue)}");

        len += 2;
        foreach (var kvp in info.DeploymentPlan)
        {
            Console.WriteLine("");
            Console.WriteLine(kvp.Key.AsColor(StringColor.Green));
            Console.WriteLine($"{"━".Multiply(30).AsBold().AsColor(StringColor.Green)}");
            Console.WriteLine(kvp.Value);
            Console.WriteLine($"{"━".Multiply(30).AsBold().AsColor(StringColor.Green)}");
            
            len += 4 + kvp.Value.Split('\n').Length;
        }

        Console.WriteLine($"{"Progress".AsBold().AsColor(StringColor.Blue)} {info.Progress}");
        len += 1;
        
        info.Start = Console.GetCursorPosition().Top - len;
    }
    
    public static void WriteRollout(RolloutInfo info)
    {
       Console.SetCursorPosition(0, info.Start);
       Writer.StartRollout(info);
    }
}

public class RolloutInfo
{
    public string ConfigName { get; set; } = "";
    public Dictionary<string, string> DeploymentPlan { get; set; } = new();
    public string Progress { get; set; } = "";

    public int Start { get; set; }
    public int End { get; set; }
}