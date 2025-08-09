namespace Orca.Lib.Models;

public class Member
{
    public string Target { get; set; }
    public MountInfo[] Mounts { get; set; }
}

public class MountInfo
{
    public string From { get; set; }
    public string To { get; set; }
}

public class ContainerMountInfo : MountInfo
{
    public string ContainerFrom { get; set; }
    public string ContainerTo { get; set; }
}