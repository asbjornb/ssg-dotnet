using System.Collections.Generic;
using Cottle;

namespace Ssg_Dotnet.LayoutTemplating;

public class FileContext : Dictionary<Value, Value>
{
    public FileContext(Dictionary<string, string> context, string content)
    {
        foreach (var item in context)
        {
            Add(item.Key, item.Value);
        }
        Add("content", content);
    }
}
