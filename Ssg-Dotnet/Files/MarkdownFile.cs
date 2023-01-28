using System.IO;
using System.Threading.Tasks;
using Markdig;
using Markdig.Syntax;

namespace Ssg_Dotnet.Files;

public sealed class MarkdownFile
{
    public MarkdownDocument Content { get; }
    
    private MarkdownFile(MarkdownDocument content)
    {
        Content = content;
    }

    public static async Task<MarkdownFile> ReadFromFile(FilePath filePath, MarkdownPipeline pipeline)
    {
        var input = await File.ReadAllTextAsync(filePath.AbsolutePath);
        var content = Markdown.Parse(input, pipeline);
        return new MarkdownFile(content);
    }
}
