using FluentAssertions;
using Markdig;

namespace Ssg_Dotnet.Test.ExplorationTest;

//Some exploration of the Markdig library to assert that it supports the features wanted
[TestFixture, Parallelizable(ParallelScope.Self)]
public class MarkDigTests
{
    [Test]
    public void ShouldProcessHeader()
    {
        const string markdownContent = "# SomeHeader\n";
        var result = Markdown.ToHtml(markdownContent);
        result.Should().NotBeNull();
        result.Should().Be("<h1>SomeHeader</h1>\n");
    }

    [Test]
    public void ShouldProcessLevel2Header()
    {
        const string markdownContent = "## SomeHeader\n";
        var result = Markdown.ToHtml(markdownContent);
        result.Should().NotBeNull();
        result.Should().Be("<h2>SomeHeader</h2>\n");
    }

    [Test]
    public void ShouldProcessList()
    {
        const string markdownContent = "* Item1\n* Item2\n";
        var result = Markdig.Markdown.ToHtml(markdownContent);
        result.Should().NotBeNull();
        result.Should().Be("<ul>\n<li>Item1</li>\n<li>Item2</li>\n</ul>\n");
    }

    [Test]
    public void ShouldProcessOrderedList()
    {
        const string markdownContent = "1. Item1\n2. Item2\n";
        var result = Markdig.Markdown.ToHtml(markdownContent);
        result.Should().NotBeNull();
        result.Should().Be("<ol>\n<li>Item1</li>\n<li>Item2</li>\n</ol>\n");
    }

    [Test]
    public void ShouldProcessLink()
    {
        const string markdownContent = "[Link](https://www.google.com)\n";
        var result = Markdig.Markdown.ToHtml(markdownContent);
        result.Should().NotBeNull();
        result.Should().Be("<p><a href=\"https://www.google.com\">Link</a></p>\n");
    }

    [Test]
    public void ShouldProcessImage()
    {
        const string markdownContent = "![Image](https://www.google.com)\n";
        var result = Markdig.Markdown.ToHtml(markdownContent);
        result.Should().NotBeNull();
        result.Should().Be("<p><img src=\"https://www.google.com\" alt=\"Image\" /></p>\n");
    }

    [Test]
    public void ShouldProcessCodeBlock()
    {
        const string markdownContent = "```\nvar x = 1;\n```\n";
        var result = Markdig.Markdown.ToHtml(markdownContent);
        result.Should().NotBeNull();
        result.Should().Be("<pre><code>var x = 1;\n</code></pre>\n");
    }

    [Test]
    public void ShouldProcessCodeBlockWithLanguage()
    {
        const string markdownContent = "```csharp\nvar x = 1;\n```\n";
        var result = Markdig.Markdown.ToHtml(markdownContent);
        result.Should().NotBeNull();
        result.Should().Be("<pre><code class=\"language-csharp\">var x = 1;\n</code></pre>\n");
    }
}
