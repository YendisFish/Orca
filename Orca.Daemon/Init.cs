using System.Diagnostics;
using Orca.Lib.Exceptions;

namespace Orca.Daemon;

public class Init
{
    public static async Task CreateGroup()
    {
        ProcessStartInfo pinf = new ProcessStartInfo()
        {
            FileName = "groupadd",
            Arguments = "orca",
            RedirectStandardOutput = false,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using Process p = Process.Start(pinf) ?? 
                          throw new ProcessFailureException("Failed to start process groupadd");

        await p.WaitForExitAsync();
    }

    public static async Task ConfigureFiles()
    {
        string[] dirs = { "/var/orca/", "/etc/orca/images/" };

        foreach (string dir in dirs)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
        
        string[]  files = { "/var/orca/orca.sock" };
    }

    public static async Task ConfigurePerms()
    {
        ProcessStartInfo pinf = new ProcessStartInfo()
        {
            FileName = "chown",
            Arguments = "root:orca /var/orca",
            RedirectStandardOutput = false,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        
        
        ProcessStartInfo pinf2 = new ProcessStartInfo()
        {
            FileName = "chmod",
            Arguments = "750 /var/orca",
            RedirectStandardOutput = false,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        
        using Process p1 =  Process.Start(pinf) ?? throw new ProcessFailureException("Failed to change orca socket permissions");
        await p1.WaitForExitAsync();
        
        using Process p2 =  Process.Start(pinf2) ?? throw new ProcessFailureException("Failed to change orca socket permissions");
        await p2.WaitForExitAsync(); 
    }
}