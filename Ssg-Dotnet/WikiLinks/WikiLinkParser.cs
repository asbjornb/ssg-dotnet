﻿using Markdig.Helpers;
using Markdig.Parsers;

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

        //Pass the link back to the processor
        processor.Inline = WikiLink.Create(linkTitle, start, slice.Start, line, column);
        return true;
    }
}
