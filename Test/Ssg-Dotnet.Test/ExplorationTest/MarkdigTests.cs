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
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("<h1>SomeHeader</h1>\n"));
    }

    [Test]
    public void ShouldProcessLevel2Header()
    {
        const string markdownContent = "## SomeHeader\n";
        var result = Markdig.Markdown.ToHtml(markdownContent);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("<h2>SomeHeader</h2>\n"));
    }

    [Test]
    public void ShouldProcessList()
    {
        const string markdownContent = "* Item1\n* Item2\n";
        var result = Markdig.Markdown.ToHtml(markdownContent);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("<ul>\n<li>Item1</li>\n<li>Item2</li>\n</ul>\n"));
    }

    [Test]
    public void ShouldProcessOrderedList()
    {
        const string markdownContent = "1. Item1\n2. Item2\n";
        var result = Markdig.Markdown.ToHtml(markdownContent);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("<ol>\n<li>Item1</li>\n<li>Item2</li>\n</ol>\n"));
    }

    [Test]
    public void ShouldProcessLink()
    {
        const string markdownContent = "[Link](https://www.google.com)\n";
        var result = Markdig.Markdown.ToHtml(markdownContent);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("<p><a href=\"https://www.google.com\">Link</a></p>\n"));
    }

    [Test]
    public void ShouldProcessImage()
    {
        const string markdownContent = "![Image](https://www.google.com)\n";
        var result = Markdig.Markdown.ToHtml(markdownContent);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("<p><img src=\"https://www.google.com\" alt=\"Image\" /></p>\n"));
    }

    [Test]
    public void ShouldProcessCodeBlock()
    {
        const string markdownContent = "```\nvar x = 1;\n```\n";
        var result = Markdig.Markdown.ToHtml(markdownContent);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("<pre><code>var x = 1;\n</code></pre>\n"));
    }

    [Test]
    public void ShouldProcessCodeBlockWithLanguage()
    {
        const string markdownContent = "```csharp\nvar x = 1;\n```\n";
        var result = Markdig.Markdown.ToHtml(markdownContent);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo("<pre><code class=\"language-csharp\">var x = 1;\n</code></pre>\n"));
    }
}
