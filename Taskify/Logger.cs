namespace Taskify;

public static class Logger
{
    public static void Status(string message) => 
        WriteInColor(message, ConsoleColor.DarkYellow);
    public static void Error(string message) => 
        WriteInColor("ERROR: " + message, ConsoleColor.Red);
    public static void Hint(string message) =>
        WriteInColor(message, ConsoleColor.Green);

    private static void WriteInColor(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}