namespace Taskify;

public static class Logger
{
    public static void Status(string message) => 
        Log(message, ConsoleColor.DarkYellow);
    public static void Error(string message) => 
        Log("ERROR: " + message, ConsoleColor.Red);
    public static void Hint(string message) =>
        Log(message, ConsoleColor.Green);

    private static void Log(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}