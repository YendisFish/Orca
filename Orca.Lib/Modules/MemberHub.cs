using Orca.Lib.Models;
using Orca.Lib.Wrappers;
using Orca.Lib.Logging;
using System.Threading.Channels;

namespace Orca.Lib.Modules;

public class MemberHub 
{
    private Channel<MemberRequest> channel = Channel.CreateUnbounded<MemberRequest>(); 
    public CancellationTokenSource cts { get; set; } = new();

    public async Task Start()
    {
        Thread t = new Thread(async () => await RunEventLoop());
        t.Start();
    }

    private async Task RunEventLoop()
    {
       while(!cts.Token.IsCancellationRequested) 
       {
           MemberRequest r = await channel.Reader.ReadAsync();

           switch(r.type) 
           {
               case MemberRequestType.BUILD:
                   {
                       _ = BuildMember(r);            
                       break;
                   }
           }
       }
    }

    private async Task BuildMember(MemberRequest r)
    {
       Logger.Console.Info("Building container");

       //todo: Actually check if image already exists
       await Images.Pull((r.data as MemberBuildData ??
                   throw new NullReferenceException()).Image);
    }

    public async Task Push(MemberRequest r)
    {
        await channel.Writer.WriteAsync(r);
    }
}


public class MemberRequest
{
    public MemberRequestType type { get; set; }
    private bool processed { get; set; }

    public MemberRequestData data { get; set; }
    private object? output { get; set; }

    public MemberRequest(MemberRequestType tp, MemberRequestData dat) 
    {
       this.type = tp;
       this.data = dat;
    }

    public async Task<T?> WaitForComplete<T>() where T: class
    {
        while(!processed) 
        {
            continue;
        }
        
        return output as T;
    }

    
}

public enum MemberRequestType 
{
    BUILD,
    EXITED,
    INFO
}

public abstract class MemberRequestData;

public class MemberBuildData : MemberRequestData
{
    public string Image { get; set; } = "";
    public List<BuildTask> BuildTasks { get; set; } = new();
}

public class MemberExitData : MemberRequestData
{

}
