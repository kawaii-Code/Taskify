namespace Taskify;

public interface IMoodlePageSource
{
    public Task<string> GetPage(string uri);
}