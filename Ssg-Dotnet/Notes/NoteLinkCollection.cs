using System.Collections.Generic;
using Cottle;

namespace Ssg_Dotnet.Notes;
//Should be used for both forward and backward linking
internal class NoteLinkCollection : List<NoteLink>
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
