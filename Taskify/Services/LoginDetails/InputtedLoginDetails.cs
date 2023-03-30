namespace Taskify.Services.LoginDetails;

public class InputtedLoginDetails : ILoginDetailsService
{
    public string Username
    {
        get
        {
            Console.WriteLine("Moodle username: ");
            string username = Console.ReadLine();
            return username;
        }
    }
    
    public string Password 
    {
        get
        {
            Console.WriteLine("Moodle password: ");
            string password = Console.ReadLine();
            return password;
        } 
    }
}