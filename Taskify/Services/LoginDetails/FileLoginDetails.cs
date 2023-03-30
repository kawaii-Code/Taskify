namespace Taskify.Services.LoginDetails;

public class FileLoginDetails : ILoginDetailsService
{
    private const string DefaultFilePath = "taskify-user-data.txt";
    
    private readonly string _filepath;

    private string _username;
    private string _password;

    public string Username
    {
        get
        {
            if (_username != null)
                return _username;
            ReadLoginDetails();
            return _username;
        }
    }

    public string Password
    {
        get
        {
            if (_password != null)
                return _password;
            ReadLoginDetails();
            return _password;
        }
    }

    public FileLoginDetails()
    {
        _filepath = DefaultFilePath;
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
            return;
        }
        
        _username = loginDetails[0];
        _password = loginDetails[1];
    }

    public static bool AreAvailable() =>
        File.Exists(DefaultFilePath);
}