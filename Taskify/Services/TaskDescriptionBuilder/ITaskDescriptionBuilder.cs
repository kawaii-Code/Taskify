namespace Taskify.Services.TaskDescriptionBuilder;

public interface ITaskDescriptionDecorator
{
    string DecorateLine(string line);
}