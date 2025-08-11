using Newtonsoft.Json;

namespace Orca.Lib.Messages;

public class OrcaMessage
{
    public MessageType Type { get; set; }

    
    [JsonConstructor]
    public OrcaMessage(MessageType type)
    {
        Type = type;
    }
}

public enum MessageType
{
    DATA,
    BUILD,
    TOGGLESTREAM
}
