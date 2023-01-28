using System.Collections.Generic;
using Cottle;

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

    //Used for adding specific entries for a given file - like backlinks
    public void AddCottleEntry(ICottleEntry cottleEntry)
    {
        (var key, var value) = cottleEntry.ToCottleContext();
        Add(key, value);
    }
}
