namespace Taskify.Services.TaskDescriptionBuilder;

public interface ITaskDescriptionBuilder
{
    string BuildLine(string line);
}