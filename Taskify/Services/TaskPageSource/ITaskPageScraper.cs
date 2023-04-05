namespace Taskify.Services.TaskPageSource;

public interface ITaskPageScraper
{
    public Task<string> GetPage(string uri);
}