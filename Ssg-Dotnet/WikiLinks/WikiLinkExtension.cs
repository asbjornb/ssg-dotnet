using Markdig;
using Markdig.Renderers;

namespace Ssg_Dotnet.WikiLinks;

internal class WikilinkExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        var parser = new WikiLinkParser();
        pipeline.InlineParsers.Insert(0, parser);
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
    }
}
