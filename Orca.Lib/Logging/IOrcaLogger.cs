namespace Orca.Lib.Logging;

public interface IOrcaLogger
{
    public void Info(string message);
    public void Error(string message);
    public void Warning(string message);
    public void Custom(string message, StringColor color);
}