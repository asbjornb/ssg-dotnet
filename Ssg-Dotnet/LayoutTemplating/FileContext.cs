using System.Collections.Generic;
using Cottle;
using Ssg_Dotnet.Notes;

namespace Ssg_Dotnet.LayoutTemplating;

internal class FileContext : Dictionary<Value, Value>
{
    public FileContext(Dictionary<string, string> context, string content)
    {
        foreach (var item in context)
        {
            Add(item.Key, item.Value);
        }
        Add("content", content);
    }

    public void AddBacklinks(NoteLinkCollection noteLinks)
    {
        (var key, var value) = noteLinks.ToCottleContext();
        Add(key, value);
    }
}
