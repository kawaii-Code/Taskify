namespace Taskify.Services.LoginDetails;

public class FileLoginDetails : ILoginDetails
{
    private const string DefaultFilePath = "taskify-user-data.txt";
    
    private readonly string _filepath;

    private string? _username;
    private string? _password;
    
    public FileLoginDetails()
    {
        _filepath = DefaultFilePath;
    }

    public string GetUsername()
    {
        if (_username == null)
            ReadLoginDetails();
        return _username;
    }

    public string GetPassword()
    {
        if (_password == null)
            ReadLoginDetails();
        return _password;
    }

    private void ReadLoginDetails()
    {
        string[] loginDetails;
        try
        {
            loginDetails = File.ReadAllText(_filepath).Split('\n');
        }
        catch (FileNotFoundException)
        {
            Logger.Error($"No file '{_filepath}' found!");
            Logger.Hint($"Please create '{_filepath}' in the same directory as the executable.");
            Logger.Hint("It should contain your username and password in the following format:");
            Logger.Hint("username");
            Logger.Hint("password");
            Logger.Hint("With no trailing spaces, etc., only a line break between them.");
            throw;
        }
        
        _username = loginDetails[0];
        _password = loginDetails[1];
    }

    public static bool AreAvailable() =>
        File.Exists(DefaultFilePath);
}