using Taskify;
using Taskify.Services.Arguments;
using Taskify.Services.LoginDetails;
using Taskify.Services.TaskDescriptionBuilder;
using Taskify.Services.TaskDescriptionScraper;
using Taskify.Services.TaskPageSource;

IArguments arguments = new ConsoleArguments(args);
if (arguments.IsHelp())
{
    Logger.Help();
    return 0;
}
ILoginDetails loginDetails = 
    FileLoginDetails.AreAvailable()
        ? new FileLoginDetails()
        : new InputtedLoginDetails();
ITaskPageSource pageSource;
ITaskDescriptionBuilder builder = new CSharpLineBuilder();

string uri = arguments.GetUri();
bool isPreview = arguments.IsPreview();

try
{
    pageSource = await InitializeMoodlePageSource();
    if (loginDetails is InputtedLoginDetails inputtedLoginDetails && inputtedLoginDetails.UserWantsToSave())
        inputtedLoginDetails.Save();
}
catch (HttpRequestException httpRequestException)
{
    Logger.Error(httpRequestException.Message);
    Logger.Error("It seems that Moodle is down right now.");
    return -1;
}
catch (Exception exception)
{
    Logger.Error(exception.Message);
    return -1;
}

Logger.Status("Extracting tasks...");
TaskDescriptionScraper descriptionScraper = new(pageSource, builder);
string tasks = await descriptionScraper.GetTaskDescriptionsAsync(uri);

if (isPreview)
{
    Logger.Preview(tasks);
}
else
{
    string? filepath = arguments.GetFilepath();
    if (filepath == null)
    {
        Logger.ReportFileNotFound();
        return -1;
    }
    WriteResultToFile(filepath, tasks);
}
return 0;

async Task<MoodlePageSource> InitializeMoodlePageSource()
{
    MoodlePageSource source = new(loginDetails);
    Logger.Status("Logging in into moodle...");
    await source.Login();
    Logger.Status("Successfully logged in!");
    return source;
}

void WriteResultToFile(string filepath, string text)
{
    Logger.Status($"Writing result to '{filepath}'...");
    using StreamWriter writer = File.AppendText(filepath);
    writer.WriteLine();
    writer.Write(text);
}