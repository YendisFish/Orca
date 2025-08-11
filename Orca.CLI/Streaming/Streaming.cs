using System.Net.Sockets;
using Newtonsoft.Json;
using Orca.Lib.Messages;
using Orca.Lib.Sockets;

namespace Orca.CLI.Streaming;

public class Streaming
{
    public static async Task IntakeStream(OrcaSocket sock)
    {
        CancellationTokenSource cts = new(TimeSpan.FromSeconds(3));
        OrcaMessage msg = await sock.Read(cts.Token) ?? 
            throw new NullReferenceException("Timed out!");
        if(msg.Type != MessageType.TOGGLESTREAM) return;
        
        string? ln;
        while((ln = await sock.reader.ReadLineAsync()) != null) 
        {
            OrcaMessage dat = JsonConvert.DeserializeObject<OrcaMessage>(ln!, Orca.Lib.Globals.Settings)
                ?? throw new NullReferenceException("Data recieved was not in the correct format!");

            
            switch(dat)
            {
                case StreamMessage sdat:
                {
                    Console.WriteLine(sdat.data);
                    break;
                }
                case ToggleStreamMessage end: return;
                default: continue;
            }
        }
    }
}

