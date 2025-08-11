using System.Text;
using Newtonsoft.Json;
using Orca.Lib.Messages;

namespace Orca.Lib.Sockets;

public class TeeWriter : TextWriter
{
    private TextWriter stdout { get; set; }
    private Stream sockStream { get; set; }
    private MemoryList<char> buff = new MemoryList<char>(50);
    private StreamWriter wrt { get; set; }

    public TeeWriter(TextWriter std, Stream sock)
    {
        stdout = std;
        sockStream = sock;
        wrt = new StreamWriter(sockStream);
        wrt.AutoFlush = true;
    }

    public override Encoding Encoding => Encoding.UTF8;

    public override void Write(char val)
    {
        switch(val)
        {
            case '\n':
            case '\r':
            case '\b':
            case '\x1b':
            {
                buff.Add(val);
                Flush();
                break;
            }
            default:
            {
                if(buff.Count > 50) 
                {
                    Flush();
                    buff.Add(val);
                    break;
                }

                buff.Add(val);

                break;
            }
        }
    }

    public override void Flush() 
    {
        stdout.Flush();
        sockStream.Flush();

        string val = buff.AsString();
        buff.Clear();

        stdout.Write(val);
    
        StreamMessage msg = new StreamMessage(MessageType.DATA, val);
        string json = JsonConvert.SerializeObject(msg, Globals.Settings);
        
        wrt.WriteLine(json);
    }

    public void ResetStdout()
    {
       Flush();

       ToggleStreamMessage msg = new ToggleStreamMessage(MessageType.TOGGLESTREAM);
       string json = JsonConvert.SerializeObject(msg);
       wrt.WriteLine(json);

       Console.SetOut(stdout);
    }

    public void OpenStream()
    {
       ToggleStreamMessage msg = new ToggleStreamMessage(MessageType.TOGGLESTREAM);
       string json = JsonConvert.SerializeObject(msg);
       wrt.WriteLine(json);
    }
}


public class MemoryList<T>
{
    public Memory<T> Mem { get; set; }
    
    private int Capacity { get; set; }
    private int Rate { get; init; }

    public int Count = 0;

    public MemoryList(int growthRate)
    {
        Capacity = growthRate;
        Rate = growthRate;
        Mem = new Memory<T>(new T[Capacity]);
    }

    public void Add(T obj)
    {
        if(Count == Capacity) 
        {
            Capacity = Capacity + Rate;
            
            Memory<T> n = new Memory<T>(new T[Capacity]);
            Mem.Slice(0, Count).CopyTo(n);

            Mem = n;
        }

        Mem.Span[Count] = obj;
        Count++;
    }

    public void Clear()
    {
        Mem = new Memory<T>(new T[Rate]);
        Capacity = Rate;
        Count = 0;
    }
}

public static class MemoryListExtensions
{
    public static string AsString(this MemoryList<char> list) 
    {
        string ret = new string(list.Mem.Span.Slice(0, list.Count));
        return ret;
    }
}
