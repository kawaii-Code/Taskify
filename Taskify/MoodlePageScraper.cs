using System.Text.RegularExpressions;

namespace Taskify;

public partial class MoodlePageScraper : IMoodlePageSource
{
    private readonly string _username;
    private readonly string _password;
    private readonly HttpClient _client;
    
    private bool _isLoggedIn;

    public MoodlePageScraper(string username, string password)
    {
        _username = username;
        _password = password;
        _client = new HttpClient();
    }

    ~MoodlePageScraper()
    {
        _client.Dispose();
    }

    private const string LoginPage = @"https://edu.mmcs.sfedu.ru/login/index.php";

    public async Task<string> GetPage(string uri)
    {
        if (!_isLoggedIn)
            throw new InvalidOperationException("You must be logged in before you scrape any pages!");
        
        HttpResponseMessage response = await _client.GetAsync(uri);
        string tasks = await response.Content.ReadAsStringAsync();
        
        return tasks;
    }
    
    public async Task Login()
    {
        string loginToken = await GetLoginToken();
        
        FormUrlEncodedContent loginDetails = new(new[]
        {
            new KeyValuePair<string, string>("username", _username),
            new KeyValuePair<string, string>("password", _password),
            new KeyValuePair<string, string>("logintoken", loginToken),
        });
        
        HttpResponseMessage loginResponse = await _client.PostAsync(LoginPage, loginDetails);
        loginResponse.EnsureSuccessStatusCode();
        
        // TODO: This check is costly
        string text = await loginResponse.Content.ReadAsStringAsync();
        if (text.Contains("loginerrormessage", StringComparison.Ordinal))
            throw new ArgumentException("Login or password are incorrect!");

        _isLoggedIn = true;
    }
    
    private async Task<string> GetLoginToken()
    {
        HttpResponseMessage login = await _client.GetAsync(LoginPage);
        string content = await login.Content.ReadAsStringAsync();
        
        Regex loginTokenPattern = GetLoginTokenRegex();
        string loginToken = loginTokenPattern.Match(content).Groups["token"].Value;
        
        return loginToken;
    }

    [GeneratedRegex("logintoken.*?value=\"(?<token>\\S+?)\"")]
    private static partial Regex GetLoginTokenRegex();
}