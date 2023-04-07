namespace Taskify.Services.LoginDetails;

public class InputtedLoginDetails : ILoginDetails
{
    private string _username;
    private string _password;

    public string GetUsername()
    {
        Console.Write("Moodle username: ");
        _username = Console.ReadLine();
        return _username;
    }
    
    public string GetPassword()
    {
        Console.Write("Moodle password: ");
        _password = Console.ReadLine();
        return _password;
    }

    public bool UserWantsToSave()
    {
        Console.Write("Save your login and password to a file? (y/n): ");
        string response = Console.ReadLine();
        return response.ToLower() == "y";
    }

    public void Save() =>
        File.WriteAllText(FileLoginDetails.UserLoginDetailsFilepath, string.Join("\n", _username, _password));
}