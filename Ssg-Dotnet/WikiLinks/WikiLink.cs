using System.Threading;
using Markdig.Helpers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Ssg_Dotnet.WikiLinks;

//Extension that mostly serves to be able to find these in the "DOM" during processing and to ease creation of LinkInLine
internal class WikiLink : LinkInline
{
    private WikiLink(string url, int start, int end, int line, int column) : base(url, "")
    {
        Span = new SourceSpan(start, end);
        Line = line;
        Column = column;
        IsClosed = true;
    }
    
    public static WikiLink Create(string url, int start, int end, int line, int column)
    {
        var link = new WikiLink(url, start, end, line, column);
        link.AppendChild(new LiteralInline()
        {
            Span = link.Span,
            Line = line,
            Column = column,
            Content = new StringSlice(url),
            IsClosed = true
        });
        return link;
    }
}
