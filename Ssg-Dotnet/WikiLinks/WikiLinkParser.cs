using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Ssg_Dotnet.WikiLinks;

internal class WikiLinkParser : InlineParser
{
    public WikiLinkParser()
    {
        OpeningCharacters = new[] { '[' };
    }

    public override bool Match(InlineProcessor processor, ref StringSlice slice)
    {
        var start = slice.Start;
        processor.GetSourcePosition(start, out var line, out var column);

        var c = slice.NextChar();
        if (c != '[')
        {
            return false;
        }

        // Skip the '['
        var current = slice.NextChar();
        var titleStart = slice.Start;

        while (current != ']')
        {
            if (current == '\0')
            {
                return false;
            }
            current = slice.NextChar();
        }
        var titleEnd = slice.Start; //Current = ']'

        if (slice.NextChar() != ']')
        {
            return false;
        }
        slice.SkipChar(); //Move past the closing ']'

        var linkTitle = slice.Text[titleStart..titleEnd];

        var link = new LinkInline(linkTitle, "")  //We don't want title. Blank is ignored and won't produce the tag
        {
            Span = new SourceSpan(start, slice.Start),
            Line = line,
            Column = column,
            IsClosed = true
        };

        link.AppendChild(new LiteralInline()
        {
            Span = link.Span,
            Line = line,
            Column = column,
            Content = new StringSlice(linkTitle),
            IsClosed = true
        });

        // Add the link to the current inline container
        processor.Inline = link;
        return true;
    }
}
