using System.Text.RegularExpressions;
using Taskify.Services.LoginDetails;

namespace Taskify.Services.TaskPageSource;

public partial class MoodlePageScraper : ITaskPageScraper
{
    private const string LoginPage = @"https://edu.mmcs.sfedu.ru/login/index.php";
    
    private readonly ILoginDetails _loginDetails;
    private readonly HttpClient _client;
    
    private bool _isLoggedIn;

    public MoodlePageScraper(ILoginDetails loginDetails)
    {
        _loginDetails = loginDetails;
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

        const string usernameField = "username";
        const string passwordField = "password";
        const string loginTokenField = "logintoken";
        FormUrlEncodedContent loginDetails = new(new[]
        {
            new KeyValuePair<string, string>(usernameField, _loginDetails.GetUsername()),
            new KeyValuePair<string, string>(passwordField, _loginDetails.GetPassword()),
            new KeyValuePair<string, string>(loginTokenField, loginToken),
        });
        
        HttpResponseMessage loginResponse = await _client.PostAsync(LoginPage, loginDetails);
        await AssertSuccessfulLogin(loginResponse);
        
        _isLoggedIn = true;
    }

    private static async Task AssertSuccessfulLogin(HttpResponseMessage loginResponse)
    {
        const string loginErrorMarker = "loginerrormessage";
        
        loginResponse.EnsureSuccessStatusCode();
        string text = await loginResponse.Content.ReadAsStringAsync();
        if (text.Contains(loginErrorMarker, StringComparison.Ordinal))
            throw new ArgumentException("Login or password are incorrect!");
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