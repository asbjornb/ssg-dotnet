using System;
using System.IO;
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
            var extension = Path.GetExtension(file);
            if (extension == ".md")
            {
                var input = await inputFileHandler.ReadFileAsync(file);
                var content = Markdown.ToHtml(input);
                //switch extention to .html for outputFile:
                var outputFile = Path.ChangeExtension(file, ".html");
                var output = templateHandler.Render("default", Path.GetFileNameWithoutExtension(file), content);
                await outputFileHandler.WriteFileAsync(outputFile, output);
            }
            else
            {
                outputFileHandler.CopyFile(file);
            }
        }
    }
}
