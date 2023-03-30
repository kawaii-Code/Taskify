namespace Taskify.Services.Arguments;

public class InputtedArguments : IArguments
{
    public string Uri
    {
        get
        {
            Console.WriteLine("Insert uri: ");
            string uri = Console.ReadLine();
            return uri;
        }
    }

    public string Filepath
    {
        get
        {
            Console.WriteLine("Insert filepath: ");
            string filepath = Console.ReadLine();
            return filepath;
        }
    }
}