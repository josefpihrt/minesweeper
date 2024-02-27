namespace DotNetGame.Minesweeper;

public static class ConsoleUtility
{
    public static void WriteLine(string value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
    {
        Write(value, foregroundColor, backgroundColor);
        Console.WriteLine();
    }
    public static void Write(string value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
    {
        if (foregroundColor is not null)
        {
            if (backgroundColor is not null)
            {
                Console.ForegroundColor = foregroundColor.Value;
                Console.BackgroundColor = backgroundColor.Value;
                Console.Write(value);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = foregroundColor.Value;
                Console.Write(value);
                Console.ResetColor();
            }
        }
        else if (backgroundColor is not null)
        {
            Console.BackgroundColor = backgroundColor.Value;
            Console.Write(value);
            Console.ResetColor();
        }
        else
        {
            Console.Write(value);
        }
    }

    public static void WriteLine(char value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
    {
        Write(value, foregroundColor, backgroundColor);
        Console.WriteLine();
    }
    public static void Write(CellDisplayFormat format)
    {
        if (format.Character != default)
            Write(format.Character, format.ForegroundColor, format.BackgroundColor);
    }

    public static void Write(char value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
    {
        if (foregroundColor is not null)
        {
            if (backgroundColor is not null)
            {
                Console.ForegroundColor = foregroundColor.Value;
                Console.BackgroundColor = backgroundColor.Value;
                Console.Write(value);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = foregroundColor.Value;
                Console.Write(value);
                Console.ResetColor();
            }
        }
        else if (backgroundColor is not null)
        {
            Console.BackgroundColor = backgroundColor.Value;
            Console.Write(value);
            Console.ResetColor();
        }
        else
        {
            Console.Write(value);
        }
    }
}
