using YamlDotNet.Serialization;

namespace Orca.CLI.Config;

public class PodConfig
{
   [YamlMember(Alias = "name")]
   public string Name { get; set; } = "";
   [YamlMember(Alias = "pod")]
   public Dictionary<string, PodMember>? Pods { get; set; }
   [YamlMember(Alias = "profile-default")]
   public string ProfileDefault { get; set; } = "prod";
}

public class PodMember
{
   [YamlMember(Alias = "path")]
   public string? Path { get; set; }
   [YamlMember(Alias = "target")]
   public string Target { get; set; } = "";
   [YamlMember(Alias = "child-of")]
   public string? ChildOf { get; set; }
   [YamlMember(Alias = "mount")]
   public Dictionary<string, string>? Mount { get; set; }
}
