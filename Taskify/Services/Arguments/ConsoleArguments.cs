namespace Taskify.Services.Arguments;

public class ConsoleArguments : IArguments
{
    public string Uri { get; private init; }
    public string Filepath { get; private init; }

    private ConsoleArguments()
    {
    }

    public static ConsoleArguments Parse(string[] arguments)
    {
        return new ConsoleArguments()
        {
            Uri = arguments[0],
            Filepath = arguments[1],
        };
    }
}