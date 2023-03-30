using System.Text.RegularExpressions;
using Taskify.Services.LoginDetails;

namespace Taskify.Services.TaskPageSource;

public partial class MoodlePageScraper : ITaskPageSource
{
    private const string LoginPage = @"https://edu.mmcs.sfedu.ru/login/index.php";
    
    private readonly string _username;
    private readonly string _password;
    private readonly HttpClient _client;
    
    private bool _isLoggedIn;

    public MoodlePageScraper(ILoginDetailsService loginDetails)
    {
        _username = loginDetails.Username;
        _password = loginDetails.Password;
        _client = new HttpClient();
    }

    ~MoodlePageScraper()
    {
        _client.Dispose();
    }

    public async Task<string> GetPage(string uri)
    {
        if (!_isLoggedIn)
            throw new InvalidOperationException("You must be logged in before you scrape any pages!");
        
        HttpResponseMessage response = await _client.GetAsync(uri);
        return await response.Content.ReadAsStringAsync();
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
        string loginPageContent = await login.Content.ReadAsStringAsync();
        
        Regex loginTokenPattern = GetLoginTokenRegex();

        return MatchLoginToken(loginTokenPattern);

        string MatchLoginToken(Regex pattern) =>
            pattern.Match(loginPageContent).Groups["token"].Value;
    }

    [GeneratedRegex("logintoken.*?value=\"(?<token>\\S+?)\"")]
    private static partial Regex GetLoginTokenRegex();
}