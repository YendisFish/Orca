namespace Orca.Lib.Models;

public class BuildTask
{
    public string image { get; set; }

    public BuildTask(string img) 
    {
        image = img;
    }
}
