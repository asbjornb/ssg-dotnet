using System.Threading.Tasks;
using Markdig;
using Ssg_Dotnet.Files;
using Ssg_Dotnet.LayoutTemplating;

namespace Ssg_Dotnet.Processor;

internal class FileProcessor
{
    private readonly TemplateHandler templateHandler;

    public FileProcessor(TemplateHandler templateHandler)
    {
        this.templateHandler = templateHandler;
    }

    public async Task ProcessFiles(string inputFolder, string outputFolder, string? layoutfolder)
    {
        await templateHandler.PrepareLayouts(layoutfolder);
        var inputFileHandler = new InputFileHandler(inputFolder);
        var outputFileHandler = new OutputFileHandler(inputFolder, outputFolder);

        foreach (var file in inputFileHandler.FindFiles())
        {
            var filePath = FilePath.FromString(file);
            if (filePath.Extension == ".md")
            {
                var input = await inputFileHandler.ReadFileAsync(file);
                var content = Markdown.ToHtml(input);
                //switch extention to .html for outputFile:
                var outputFile = filePath.ToIndexHtml();
                var output = templateHandler.Render("default", filePath.FileName, content);
                await outputFileHandler.WriteFileAsync(outputFile.RelativePath, output);
            }
            else
            {
                outputFileHandler.CopyFile(file);
            }
        }
    }
}
