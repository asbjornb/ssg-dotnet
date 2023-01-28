using System.Collections.Generic;
using Cottle;
using Ssg_Dotnet.LayoutTemplating;

namespace Ssg_Dotnet.Notes;
internal class NoteLinkCollection : List<NoteLink>, ICottleEntry
{
    public (Value, Value) ToCottleContext()
    {
        var values = new Value[Count];
        for (int i = 0; i<Count; i++)
        {
            values[i] = new Dictionary<Value, Value>() { { "Url", this[i].Url}, { "Title", this[i].Title }, { "Preview", this[i].Preview } };
        }
        return ("backlinks", values);
    }
}
