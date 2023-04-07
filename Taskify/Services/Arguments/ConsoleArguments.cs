namespace Taskify.Services.Arguments;

public class ConsoleArguments : IArguments
{
    private readonly string[] _args;
    
    public ConsoleArguments(string[] args)
    {
        _args = args;
    }

    public string GetUri() =>
        _args[0];

    public string? GetFilepath()
    {
        int filepathOptionIndex = Array.IndexOf(_args, "-f");
        if (filepathOptionIndex == -1)
            return null;
        return _args[filepathOptionIndex + 1];
    }

    public bool IsPreview() =>
        _args.Contains("-p");

    public bool IsHelp() =>
        _args.Contains("-h") || _args.Length == 0;
}