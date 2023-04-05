namespace Taskify.Services.Arguments;

public class InputtedArguments : IArguments
{
    public string GetUri()
    {
        Console.Write("Moodle username > ");
        string username = Console.ReadLine();
        return username;
    }

    public string? GetFilepath()
    {
        Console.Write("File to write to > ");
        string filepath = Console.ReadLine();
        return filepath;
    }

    public bool IsPreview()
    {
        Console.Write("Show preview? y/n > ");
        string response = Console.ReadLine();
        return response?.ToLower() == "y";
    }
}