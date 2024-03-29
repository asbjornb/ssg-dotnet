﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using Markdig.Syntax;

namespace Ssg_Dotnet.Files;

public sealed class MarkdownFile
{
    private readonly FilePath path;

    public MarkdownDocument Content { get; }
    public string RelativeUrl => path.RelativeUrl;

    private MarkdownFile(FilePath path, MarkdownDocument content)
    {
        this.path = path;
        Content = content;
    }

    public static async Task<MarkdownFile> ReadFromFile(FilePath filePath, MarkdownPipeline pipeline)
    {
        var input = await File.ReadAllTextAsync(filePath.AbsolutePath);
        var content = Markdown.Parse(input, pipeline);
        return new MarkdownFile(filePath, content);
    }

    public string GetPreview()
    {
        var asHtml = Content.ToHtml();
        return asHtml.Length > 1000 ? asHtml[0..1000] : asHtml;
    }

    public string ToHtml()
    {
        return Content.ToHtml();
    }

    public IEnumerable<T> GetDescendants<T>()
    {
        return Content.Descendants().OfType<T>();
    }
}
