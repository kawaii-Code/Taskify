using System.Text;

using Taskify;

if (args.Length != 2)
{
    Logger.Error("Please provide an uri and a file path!");
    Logger.Hint("Example: 'taskify https://www.youtube.com ./Program.cs'");
    return -1;
}

string uri = args[0];
string? filepath = args[1];

const string loginDetailsFilepath = "taskify-user-data.txt";
string[] loginDetails;
try
{
    loginDetails = File.ReadAllText(loginDetailsFilepath).Split('\n');
}
catch (FileNotFoundException)
{
    Logger.Error($"No file '{loginDetailsFilepath}' found!");
    Logger.Hint($"Please create '{loginDetailsFilepath}' in the same directory as the executable.");
    Logger.Hint("It should contain your username and password in the following format:");
    Logger.Hint("username");
    Logger.Hint("password");
    Logger.Hint("With no trailing spaces, etc., only a line break between them.");
    return -1;
}

string username = loginDetails[0];
string password = loginDetails[1];
MoodlePageScraper pageScraper = new(username, password);
TaskExtractor extractor = new(pageScraper);

Logger.Status("Logging in into moodle...");
try
{
    await pageScraper.Login();
}
catch (Exception e)
{
    Logger.Error(e.Message);
    return -1;
}

Logger.Status("Extracting tasks...");
string text = await extractor.GetTaskDescriptionsAsync(uri, DecorateLine);

if (filepath == null)
    throw new InvalidOperationException("The filepath is null!");

Logger.Status($"Writing result to '{filepath}'...");
await using (StreamWriter writer = File.AppendText(filepath))
{
    writer.WriteLine();
    writer.Write(text);
}

Logger.Hint("Successfully finished!");
return 0;

string DecorateLine(string line)
{
    StringBuilder csLineBuilder = new();
    csLineBuilder.AppendLine("// == TASK 1 ==");
    csLineBuilder.Append("// ");
    csLineBuilder.AppendLine(line);
    
    return csLineBuilder.ToString();
}