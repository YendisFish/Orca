namespace Orca.Lib.Exceptions;

public class ProcessFailureException : Exception
{
    public new string Message { get; set;  }

    public ProcessFailureException(string msg)
    {
        Message = msg;    
    }

    public override string ToString()
    {
        return $"""
                {"Orca encountered an error!".AsBold().AsColor(StringColor.Red)}
                {"Message".AsBold().AsColor(StringColor.Red)}: {Message}
                """;
    }
}