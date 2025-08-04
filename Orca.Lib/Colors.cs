namespace Orca.Lib;

public static class StringDecorations
{
    public static string Multiply(this string str, int multiplier = 2)
    {
        string ret = str;
        for (int i = 0; i < multiplier; i++)
        {
            ret = ret + str;
        }
        
        return ret;
    }

    public static string AsColor(this string str, StringColor color)
    {
        string unicode = color.GetUnicode();
        return unicode + str + StringColor.White.GetUnicode();
    }
    
    public static string AsBold(this string str)
    {
        return "\u001b[1m" + str + "\u001b[0m";
    }

    private static string GetUnicode(this StringColor color)
    {
        return color switch
        {
            StringColor.Red => "\u001b[31m",
            StringColor.Orange => "\u001b[33m",
            StringColor.Yellow => "\u001b[93m",
            StringColor.Green => "\u001b[92m",
            StringColor.Blue => "\u001b[94m",
            StringColor.Purple => "\u001b[35m",
            StringColor.Magenta => "\u001b[95m",
            StringColor.Black => "\u001b[30m",
            StringColor.White => "\u001b[0m",
            _ => "0m"
        };
    }
}

public enum StringColor
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Purple,
    Magenta,
    Black,
    White
}