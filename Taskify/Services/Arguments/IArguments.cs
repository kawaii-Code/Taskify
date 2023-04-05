namespace Taskify.Services.Arguments;

public interface IArguments
{
    string GetUri();
    string? GetFilepath();
    bool IsPreview();
}