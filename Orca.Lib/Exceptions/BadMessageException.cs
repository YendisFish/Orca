namespace Orca.Lib.Exceptions;

public class BadMessageException : Exception
{
    public new string Message { get; set;  }

    public BadMessageException(string msg)
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