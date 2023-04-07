using System.Text;

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
        
        StringBuilder passwordBuilder = new();
        ConsoleKeyInfo next;
        do
        {
            next = Console.ReadKey(true);
            if (char.IsLetterOrDigit(next.KeyChar) || char.IsPunctuation(next.KeyChar))
            {
                Console.Write("*");
                passwordBuilder.Append(next.KeyChar);
            }

            if (next.Key == ConsoleKey.Backspace)
            {
                Console.Write("\b");
                passwordBuilder.Remove(passwordBuilder.Length - 1, 1);
            }
        } while (next.Key != ConsoleKey.Enter);
        Console.WriteLine();
        
        _password = passwordBuilder.ToString().Trim();
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