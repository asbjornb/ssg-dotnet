using Markdig;
using Ssg_Dotnet.WikiLinks;

namespace Ssg_Dotnet.Test.WikiLinksTests;

[TestFixture, Parallelizable(ParallelScope.Self)]
internal class ExtensionTests
{
    //Test that the extension can parse a simple wikilink
    [Test]
    [TestCase("Ab [[SomeLink]] ba", "<p>Ab <a href=\"SomeLink\">SomeLink</a> ba</p>\n")]
    public void ShouldParseSimpleWikilink(string markdownContent, string expectedHtml)
    {
        var pipelineBuiler = new MarkdownPipelineBuilder();
        var extension = new WikilinkExtension();
        extension.Setup(pipelineBuiler);
        var pipeline = pipelineBuiler.Build();

        var result = Markdown.ToHtml(markdownContent, pipeline);
        result.Should().Be(expectedHtml);
    }

    //Test that we can parse a standard link
    [Test]
    public void ShouldParseSimpleLink()
    {
        const string markdownContent = "[SomeAnchorText](SomeUrl)";

        var result = Markdown.ToHtml(markdownContent);
        result.Should().NotBeNull();
        result.Should().Be("<p><a href=\"SomeUrl\">SomeAnchorText</a></p>\n");
    }

    //Test that the extension doesn't break standard links
    [Test]
    public void ExtensionShouldParseSimpleLink()
    {
        const string markdownContent = "[SomeAnchorText](SomeUrl)";
        var pipelineBuiler = new MarkdownPipelineBuilder();
        var extension = new WikilinkExtension();
        extension.Setup(pipelineBuiler);
        var pipeline = pipelineBuiler.Build();

        var result = Markdown.ToHtml(markdownContent, pipeline);
        result.Should().NotBeNull();
        result.Should().Be("<p><a href=\"SomeUrl\">SomeAnchorText</a></p>\n");
    }
}
