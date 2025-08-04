using Orca.Lib.Exceptions;

namespace Orca.Lib.Wrappers;

public class Images
{
    public static async Task Pull(string image)
    {
        using var sckopeo = await Scripting.Execute("skopeo", "", new ProcessFailureException("Failed to pull image " + image));
        await sckopeo.WaitForExitAsync();
    }
}