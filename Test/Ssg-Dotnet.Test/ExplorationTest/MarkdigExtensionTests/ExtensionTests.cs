using Markdig;
using Ssg_Dotnet.WikiLinks;

namespace Ssg_Dotnet.Test.ExplorationTest.MarkdigExtensionTests;

[TestFixture, Parallelizable(ParallelScope.Self)]
internal class ExtensionTests
{
    //Test that the extension can parse a simple wikilink
    [Test]
    public void ShouldParseSimpleWikilink()
    {
        const string markdownContent = "Ab [[SomeLink]] ba";
        var pipelineBuiler = new MarkdownPipelineBuilder();
        var extension = new WikilinkExtension();
        extension.Setup(pipelineBuiler);
        var pipeline = pipelineBuiler.Build();

        var result = Markdown.ToHtml(markdownContent, pipeline);
        result.Should().NotBeNull();
        result.Should().Be("<p>Ab <a href=\"SomeLink\">SomeLink</a> ba</p>\n");
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
