using System.Text;

namespace Taskify.Services.TaskDescriptionBuilder;

public class CSharpLineDecorator : ITaskDescriptionDecorator
{
    public string DecorateLine(string line)
    {
        StringBuilder csLineBuilder = new();
        csLineBuilder.AppendLine("// == TASK 1 ==");
        csLineBuilder.Append("// ");
        csLineBuilder.AppendLine(string.Join("\n// ", line.Split("\n")));
        return csLineBuilder.ToString();
    }
}