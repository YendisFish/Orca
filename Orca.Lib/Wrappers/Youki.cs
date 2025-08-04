namespace Orca.Lib.Wrappers;

public class Youki
{
    public static bool Installed() => (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator, 
            StringSplitOptions.RemoveEmptyEntries).Any(x => File.Exists(Path.Combine(x, "youki")));
}