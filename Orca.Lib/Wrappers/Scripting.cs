using System.Diagnostics;

namespace Orca.Lib.Wrappers;

public class Scripting
{
    public static async Task<Process> Execute(string path, string args, Exception failure)
    {
        ProcessStartInfo pinf = new ProcessStartInfo()
        {
            FileName = path,
            Arguments = args,
            RedirectStandardOutput = false,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        
        Process p = Process.Start(pinf) ?? throw failure;
        return p;
    }
}