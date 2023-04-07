using System.Text;

namespace Taskify.Services.TaskDescriptionBuilder;

public class CSharpLineBuilder : ITaskDescriptionBuilder
{
    private int _taskIndex = 1;
    
    public string BuildLine(string line)
    {
        StringBuilder csLineBuilder = new();
        csLineBuilder.AppendLine($"// == TASK {_taskIndex++} ==");
        csLineBuilder.Append("// ");
        csLineBuilder.AppendLine(string.Join("\n// ", line.Split("\n")));
        return csLineBuilder.ToString();
    }
}