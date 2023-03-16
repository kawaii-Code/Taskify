using System.Text;
using System.Text.RegularExpressions;

namespace Taskify;

public partial class TaskExtractor
{
    private readonly IMoodlePageSource _source;
    private readonly Regex _taskPattern;
    private readonly Regex[] _filters;
    
    public TaskExtractor(IMoodlePageSource source)
    {
        _source = source;
        _taskPattern = GetDefaultTaskPattern();
        
        Regex htmlTag = new(@"<.*?>");
        Regex taskName = new(@"\[.+?\]");
        Regex xmlEscapeSequence = new(@"&.+?;");
        _filters = new[] { htmlTag, taskName, xmlEscapeSequence };
    }
    
    [GeneratedRegex(@"<ol.*?>[\s\S]*?</ol>")]
    public static partial Regex GetDefaultTaskPattern();

    public async Task<string> GetTaskDescriptionsAsync(string uri, Func<string, string> decorateLine)
    {
        return await GetTaskDescriptionsAsync(uri, _taskPattern, decorateLine);
    }
    
    public async Task<string> GetTaskDescriptionsAsync(string uri, Regex taskPattern, Func<string, string> decorateLine)
    {
        StringBuilder resultBuilder = new();
        string page = await _source.GetPage(uri);
        
        foreach (Match match in taskPattern.Matches(page))
        {
            string section = match.Value;
            foreach (Regex filter in _filters)
                section = filter.Replace(section, "");
            
            string[] descriptions = section.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            foreach (string description in descriptions)
            {
                string resultLine = decorateLine(description.Trim());
                resultBuilder.AppendLine(resultLine);
            }
        }

        return resultBuilder.ToString();
    }
}