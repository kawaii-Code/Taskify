using System.Text;
using System.Text.RegularExpressions;

if (args.Length != 2)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERROR: Please provide an uri and a file path!");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Example: 'taskify https://www.youtube.com ./Program.cs'");
    Console.ResetColor();
    return -1;
}

string uri = args[0];
string filepath = args[1];

HttpClient client = new();
Regex textPattern = new(@"<ol>[\s\S]*?<\/ol>");
Regex htmlTag = new(@"<.*?>");
Regex taskNames = new(@"\[.+?\]");
Regex weirdSymbols = new(@"&.+?;");
Regex[] filters = { htmlTag, taskNames, weirdSymbols };

Console.WriteLine($"Getting text from '{uri}'...");
string text = await GetTaskDescriptionsAsync(uri, line =>
{
    StringBuilder csLineBuilder = new();
    csLineBuilder.AppendLine("// == TASK 1 ==");
    csLineBuilder.Append("// ");
    csLineBuilder.AppendLine(line);
    
    return csLineBuilder.ToString();
});

Console.WriteLine($"Writing result to '{filepath}'...");
await using (StreamWriter writer = File.AppendText(filepath))
{
    writer.WriteLine();
    writer.Write(text);
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Successfully finished!");
return 0;

async Task<string> GetTaskDescriptionsAsync(string uri, Func<string, string> decorateLine)
{
    StringBuilder resultBuilder = new();

    HttpResponseMessage response = await client.GetAsync(uri);
    response.EnsureSuccessStatusCode();
    string text = await response.Content.ReadAsStringAsync();

    foreach (Match match in textPattern.Matches(text))
    {
        string section = match.Value;
        foreach (var filter in filters)
            section = filter.Replace(section, "");
        
        string[] descriptions = section.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        foreach (var description in descriptions)
        {
            string resultLine = decorateLine(description.Trim());
            resultBuilder.AppendLine(resultLine);
        }
    }

    return resultBuilder.ToString();
}