namespace Taskify.Services.LoginDetails;

public class InputtedLoginDetails : ILoginDetails
{
    public string GetUsername()
    {
        Console.Write("Moodle username: ");
        string username = Console.ReadLine();
        return username;
    }
    
    public string GetPassword()
    {
        Console.Write("Moodle password: ");
        string password = Console.ReadLine();
        return password;
    }
}