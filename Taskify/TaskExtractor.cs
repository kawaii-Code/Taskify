using System.Text;
using System.Text.RegularExpressions;
using Taskify.Services.TaskDescriptionBuilder;
using Taskify.Services.TaskPageSource;

namespace Taskify;

public partial class TaskExtractor
{
    private readonly ITaskPageScraper _scraper;
    private readonly ITaskDescriptionDecorator _taskDescriptionDecorator;
    private readonly Regex _taskPattern;
    private readonly Regex[] _filters;
    
    public TaskExtractor(ITaskPageScraper scraper, ITaskDescriptionDecorator taskDescriptionDecorator)
    {
        _scraper = scraper;
        _taskDescriptionDecorator = taskDescriptionDecorator;
        _taskPattern = DefaultTaskDescription();
        _filters = new[] { HtmlTagRegex(), TaskNameRegex(), XmlEscapeSequenceRegex() };
    }
    
    public async Task<string> GetTaskDescriptionsAsync(string uri) =>
        await GetTaskDescriptionsAsync(uri, _taskPattern);

    public async Task<string> GetTaskDescriptionsAsync(string uri, Regex taskPattern)
    {
        StringBuilder resultBuilder = new();
        string page = await _scraper.GetPage(uri);
        
        foreach (Match match in taskPattern.Matches(page))
        {
            string section = match.Value;
            foreach (Regex filter in _filters)
                section = filter.Replace(section, "");
            
            string[] descriptions = section.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (string description in descriptions)
            {
                string resultLine = _taskDescriptionDecorator.DecorateLine(description.Trim());
                resultBuilder.AppendLine(resultLine);
            }
        }

        return resultBuilder.ToString();
    }

    [GeneratedRegex(@"<ol.*?>[\s\S]*?</ol>")]
    private static partial Regex DefaultTaskDescription();
    
    [GeneratedRegex("<.*?>")]
    private static partial Regex HtmlTagRegex();
    [GeneratedRegex("\\[.+?\\]")]
    private static partial Regex TaskNameRegex();
    [GeneratedRegex("&.+?;")]
    private static partial Regex XmlEscapeSequenceRegex();
}