using System.Reflection;

namespace Taskify.Services.LoginDetails;

public class FileLoginDetails : ILoginDetails
{
    public const string UserLoginDetailsFileName = "taskify-user-data.txt";

    private string? _username;
    private string? _password;

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
        string[] loginDetails = File.ReadAllText(UserLoginDetailsFileName).Split('\n');
        _username = loginDetails[0];
        _password = loginDetails[1];
    }

    public static bool AreAvailable() =>
        File.Exists(UserLoginDetailsFilepath);

    public static readonly string UserLoginDetailsFilepath =
        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), UserLoginDetailsFileName);
}