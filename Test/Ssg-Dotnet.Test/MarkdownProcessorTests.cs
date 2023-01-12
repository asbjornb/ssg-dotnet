using Ssg_Dotnet.Processors;

namespace Ssg_Dotnet.Test;

[TestFixture, Parallelizable(ParallelScope.Self)]
public class MarkdownProcessorTests
{
    [Test]
    public void ShouldProcessEmptyContent()
    {
        const string markdownContent = "";
        var sut = new MarkdownProcessor();
        var result = sut.ParseToHtmlContent(markdownContent);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(""));
    }
}
