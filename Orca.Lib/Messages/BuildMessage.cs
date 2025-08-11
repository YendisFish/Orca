using Orca.Lib.Modules;

namespace Orca.Lib.Messages;

public class BuildMessage : OrcaMessage
{
    public List<MemberRequest> Requests { get; set; }

    public BuildMessage(MessageType tp) : base(tp) {}
}
