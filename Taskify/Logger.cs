﻿namespace Taskify;

public static class Logger
{
    private const string HelpPrompt = @"Welcome to Taskify!
The program requires a uri and options to work.
Please specify them like so from the command line:

~/your-project-folder/
> taskify <uri> <options>

Options:
-f <filepath> | Filepath to write the task descriptions to (Program.cs, most of the time).
-p            | Preview what was generated by taskify in the console.
-h            | Print this help prompt

Examples: 
> taskify https://edu.mmcs.sfedu.ru/mod/assign/view.php?id=14707 -f ./Example.cs
> taskify https://edu.mmcs.sfedu.ru/mod/assign/view.php?id=12403 -p";
    
    public static void Status(string message) => 
        Log(message, ConsoleColor.DarkYellow);
    public static void Error(string message) => 
        Log("ERROR: " + message, ConsoleColor.Red);
    public static void Hint(string message) =>
        Log(message, ConsoleColor.Green);
    public static void Help() =>
        Log(HelpPrompt, ConsoleColor.Gray);

    private static void Log(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}