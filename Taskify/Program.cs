using Taskify;
using Taskify.Services.Arguments;
using Taskify.Services.LoginDetails;
using Taskify.Services.TaskDescriptionBuilder;
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
ITaskPageScraper pageScraper;
try
{
    pageScraper = await InitializeMoodlePageScraper();
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
ITaskDescriptionDecorator decorator = new CSharpLineDecorator();

string uri = arguments.GetUri();
bool isPreview = arguments.IsPreview();

Logger.Status("Extracting tasks...");
TaskExtractor extractor = new(pageScraper, decorator);
string tasks = await extractor.GetTaskDescriptionsAsync(uri);

if (isPreview)
{
    PrintPreview(tasks);
}
else
{
    string? filepath = arguments.GetFilepath();
    if (filepath == null)
    {
        Logger.Error("No filepath provided!");
        return -1;
    }
    WriteResultToFile(filepath, tasks);
}

Logger.Status("Successfully finished!");
return 0;

async Task<MoodlePageScraper> InitializeMoodlePageScraper()
{
    MoodlePageScraper scraper = new(loginDetails);
    Logger.Status("Logging in into moodle...");
    await scraper.Login();
    return scraper;
}

void PrintPreview(string text)
{
    Logger.Hint("== START PREVIEW ==");
    Logger.Status(text);
    Logger.Hint("==  END PREVIEW  ==");
}

void WriteResultToFile(string filepath, string text)
{
    Logger.Status($"Writing result to '{filepath}'...");
    
    using StreamWriter writer = File.AppendText(filepath);
    writer.WriteLine();
    writer.Write(text);
}