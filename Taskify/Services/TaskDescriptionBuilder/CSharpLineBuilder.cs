using System.Text;

namespace Taskify.Services.TaskDescriptionBuilder;

public class CSharpLineDecorator : ITaskDescriptionDecorator
{
    private int _taskIndex = 1;
    
    public string DecorateLine(string line)
    {
        StringBuilder csLineBuilder = new();
        csLineBuilder.AppendLine($"// == TASK {_taskIndex++} ==");
        csLineBuilder.Append("// ");
        csLineBuilder.AppendLine(string.Join("\n// ", line.Split("\n")));
        return csLineBuilder.ToString();
    }
}