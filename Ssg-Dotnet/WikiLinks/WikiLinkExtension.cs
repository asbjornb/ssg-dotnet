using Markdig;
using Markdig.Renderers;

namespace Ssg_Dotnet.WikiLinks;

internal class WikilinkExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        if (!pipeline.InlineParsers.Contains<WikiLinkParser>())
        {
            //It seems if I add to end of list my tests break
            pipeline.InlineParsers.Insert(0, new WikiLinkParser());
        }
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
    }
}

public static class MarkdownPipeLineBuilderExtensions
{
    public static MarkdownPipelineBuilder UseWikiLinks(this MarkdownPipelineBuilder pipeline)
    {
        pipeline.Extensions.AddIfNotAlready<WikilinkExtension>();
        return pipeline;
    }
}
