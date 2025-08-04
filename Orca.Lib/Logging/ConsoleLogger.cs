namespace Orca.Lib.Logging;

public class ConsoleLogger : IOrcaLogger
{
   public void Info(string message) => Custom(message, StringColor.Green);
   public void Error(string message) => Custom(message, StringColor.Red);
   public void Warning(string message) => Custom(message, StringColor.Yellow);

   public void Custom(string message, StringColor color)
   {
      DateTime time = DateTime.UtcNow;
      Console.WriteLine($"[{time.ToString("G")}]".AsColor(color).AsBold() + $": {message}");
   }
}