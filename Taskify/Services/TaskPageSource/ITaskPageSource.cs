namespace Taskify.Services.TaskPageSource;

public interface ITaskPageSource
{
    public Task<string> GetPage(string uri);
}