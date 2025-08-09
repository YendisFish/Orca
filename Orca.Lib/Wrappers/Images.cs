using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Orca.Lib.Exceptions;
using Orca.Lib.Logging;

namespace Orca.Lib.Wrappers;

public class Images
{
    public static async Task<string> Pull(string image)
    {
        string previous = Directory.GetCurrentDirectory();
        string dirName = Path.Join("/var/orca/images/", Guid.NewGuid().ToString());
        
        Directory.CreateDirectory(dirName);
        Directory.SetCurrentDirectory(dirName);
        
        Logger.Console.Info($"Pulling image {image.AsColor(StringColor.Yellow)}");
        using var skopeo = await Scripting.Execute("skopeo", $"copy docker://{image} dir:{dirName}", 
            new ProcessFailureException("Failed to pull image " + image));
        await skopeo.WaitForExitAsync();
        
        Logger.Console.Info($"Computing image hash...");
        string hash = GetManifestHash(await File.ReadAllTextAsync("./manifest.json"));
        
        ImagesConfig conf = await ImagesConfig.GetConfig();
        conf.Images.Add(new ImageInfo(image, hash));
        await ImagesConfig.SetConfig(conf);
       
        string newDirName = Path.Join("/var/orca/images/", hash);
        Directory.CreateDirectory(newDirName);
        foreach (var fle in new DirectoryInfo(dirName).EnumerateFiles())
        {
            fle.CopyTo(Path.Join(newDirName, fle.Name), true);
        }
       
        Logger.Console.Info($"Cleaning up...");
        Directory.Delete(dirName, true);
        Logger.Console.Info("Cleaned up");
        
        Directory.SetCurrentDirectory(previous);

        return hash;
    }

    public static string GetManifestHash(string manifest)
    {
       SHA256 sha256 = SHA256.Create();
       byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(manifest));
       return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    }
}

public record ImagesConfig(List<ImageInfo> Images)
{
    public static async Task<ImagesConfig> GetConfig()
    {
        try
        {
            if (!File.Exists("/etc/orca/images/images.json"))
            {
                File.Create("/etc/orca/images/images.json").Close();
                var config = new ImagesConfig(new List<ImageInfo>());
                await SetConfig(config);
                
                return config;
            }

            return JsonConvert.DeserializeObject<ImagesConfig>(
                       await File.ReadAllTextAsync("/etc/orca/images/images.json"))
                   ?? new ImagesConfig(new List<ImageInfo>());
        }
        catch (Exception e)
        {
            Logger.Console.Error(e.ToString());
        }

        throw new NullReferenceException();
    }

    public static async Task SetConfig(ImagesConfig config)
    {
        try
        {
            if (!File.Exists("/etc/orca/images/images.json"))
            {
                File.Create("/etc/orca/images/images.json").Close();
            }
            
            await File.WriteAllTextAsync("/etc/orca/images/images.json", JsonConvert.SerializeObject(config));
        }
        catch (Exception e)
        {
            Logger.Console.Error(e.ToString());
        }
    }
}
public record ImageInfo(string Tag = "", string Hash = "");
