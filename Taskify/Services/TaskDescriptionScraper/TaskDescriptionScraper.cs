using System.Text;
using System.Text.RegularExpressions;
using Taskify.Services.TaskDescriptionBuilder;
using Taskify.Services.TaskPageSource;
using HtmlAgilityPack;

namespace Taskify.Services.TaskDescriptionScraper;

public partial class TaskDescriptionScraper
{
    private const string DefaultXPath = "//div[@class='no-overflow']//ol/li";
    private const string FallbackXPath = "//div[@class='no-overflow']";
    
    private readonly ITaskPageSource _source;
    private readonly ITaskDescriptionBuilder _taskDescriptionBuilder;
    private readonly Regex[] _filters;
    
    public TaskDescriptionScraper(ITaskPageSource source, ITaskDescriptionBuilder taskDescriptionBuilder)
    {
        _source = source;
        _taskDescriptionBuilder = taskDescriptionBuilder;
        _filters = new[] { HtmlTagRegex(), TaskNameRegex(), XmlEscapeSequenceRegex() };
    }
    
    public async Task<string> GetTaskDescriptionsAsync(string uri)
    {
        const int smallestTaskDescription = 20;
        
        string page = await _source.GetPage(uri);
        HtmlDocument html = new();
        html.LoadHtml(page);

        StringBuilder resultBuilder = new();
        HtmlNodeCollection nodes =
            html.DocumentNode.SelectNodes(DefaultXPath) 
            ?? html.DocumentNode.SelectNodes(FallbackXPath);

        foreach (HtmlNode taskNode in nodes)
        {
            string text = taskNode.InnerText;
            foreach (Regex filter in _filters)
                text = filter.Replace(text, " ");

            if (text.Length < smallestTaskDescription)
                continue;

            string resultLine = _taskDescriptionBuilder.BuildLine(text.Trim());
            resultBuilder.AppendLine(resultLine);
        }

        return resultBuilder.ToString();
    }

    [GeneratedRegex("<.*?>")]
    private static partial Regex HtmlTagRegex();
    [GeneratedRegex("\\[.+?\\]")]
    private static partial Regex TaskNameRegex();
    [GeneratedRegex("&.+?;")]
    private static partial Regex XmlEscapeSequenceRegex();
}