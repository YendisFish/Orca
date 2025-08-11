namespace Orca.Lib.Messages;

public class ToggleStreamMessage : OrcaMessage
{
    public ToggleStreamMessage(MessageType tp) : base(tp) {}
}

public class StreamMessage : OrcaMessage
{
    public string data { get; set; }
    public StreamMessage(MessageType tp, string data) : base(tp)
    {
        this.data = data;
    }
}
