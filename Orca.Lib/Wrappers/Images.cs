using System.Diagnostics;
using Orca.Lib.Exceptions;
using Orca.Lib.Logging;

namespace Orca.Lib.Wrappers;

public class Images
{
    public static async Task Pull(string image)
    {
        Logger.Console.Info($"Pulling image {image.AsColor(StringColor.Yellow)}");
        
        string previous = Directory.GetCurrentDirectory();
        string dirName = Path.Join("/var/orca/images/", Guid.NewGuid().ToString());
        
        Directory.CreateDirectory(dirName);
        Directory.SetCurrentDirectory(dirName);
        
        using var sckopeo = await Scripting.Execute("skopeo", $"copy docker://{image} dir:{dirName}", 
            new ProcessFailureException("Failed to pull image " + image));
        await sckopeo.WaitForExitAsync();

        foreach (var dir in new DirectoryInfo(Directory.GetCurrentDirectory()).GetDirectories())
        {
            Console.WriteLine(dir.Name);
        } 
        
        Directory.SetCurrentDirectory(previous);
    }
}