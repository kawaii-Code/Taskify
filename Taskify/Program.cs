using Taskify;
using Taskify.Services.Arguments;
using Taskify.Services.LoginDetails;
using Taskify.Services.TaskDescriptionBuilder;
using Taskify.Services.TaskPageSource;

IArguments arguments = 
    args.Length > 1
        ? ConsoleArguments.Parse(args)
        : new InputtedArguments();
ILoginDetailsService loginDetails = 
    FileLoginDetails.AreAvailable()
        ? new FileLoginDetails()
        : new InputtedLoginDetails();
ITaskPageSource pageSource = await GetMoodlePageScraper(loginDetails);
ITaskDescriptionDecorator decorator = new CSharpLineDecorator();
TaskExtractor extractor = new(pageSource, decorator);

string uri = arguments.Uri;
string filepath = arguments.Filepath;

Logger.Status("Extracting tasks...");
string text = await extractor.GetTaskDescriptionsAsync(uri);

Logger.Status($"Writing result to '{filepath}'...");
await using (StreamWriter writer = File.AppendText(filepath))
{
    writer.WriteLine();
    writer.Write(text);
}

Logger.Status("Successfully finished!");
return 0;

async Task<MoodlePageScraper> GetMoodlePageScraper(ILoginDetailsService loginDetailsService)
{
    MoodlePageScraper pageScraper = new(loginDetailsService);
    Logger.Status("Logging in into moodle...");
    try
    {
        await pageScraper.Login();
    }
    catch (Exception e)
    {
        Logger.Error(e.Message);
        throw;
    }

    return pageScraper;
}