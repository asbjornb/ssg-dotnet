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

    public void AddBacklinks(List<NoteLink> noteLinks)
    {
        var values = new Value[noteLinks.Count];
        for (int i = 0; i < noteLinks.Count; i++)
        {
            values[i] = new Dictionary<Value, Value>() { { "Url", noteLinks[i].Url }, { "Title", noteLinks[i].Title }, { "Preview", noteLinks[i].Preview } };
        }
        Add("backlinks", values);
    }
}
