using System.Collections.Generic;
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

    public async Task ProcessFiles(string inputFolder, string outputFolder, string? notesFolder, string? layoutfolder)
    {
        await templateHandler.PrepareLayouts(layoutfolder);
        var inputFileHandler = new InputFileHandler(inputFolder);
        var outputFileHandler = new OutputFileHandler(inputFolder, outputFolder);
        await ProcessFolder(inputFileHandler, outputFileHandler, isNote: false);
        if (notesFolder != null)
        {
            var notesFileHandler = new InputFileHandler(notesFolder);
            var notesOutputHandler = new OutputFileHandler(notesFolder, Path.Combine(outputFolder, "notes"));
            await ProcessFolder(notesFileHandler, notesOutputHandler, isNote: true);
        }
    }

    private async Task ProcessFolder(InputFileHandler inputHandler, OutputFileHandler outputHandler, bool isNote)
    {
        foreach (var file in inputHandler.FindFiles())
        {
            var filePath = FilePath.FromString(file);
            if (filePath.Extension == ".md")
            {
                var input = await inputHandler.ReadFileAsync(file);
                var content = Markdown.ToHtml(input);
                //switch extention to .html for outputFile:
                var outputFile = filePath.ToIndexHtml();
                string output;
                if (isNote)
                {
                    output = templateHandler.RenderNote("default", filePath.FileName, content, new List<string>());
                }
                else
                {
                    output = templateHandler.Render("default", filePath.FileName, content);
                }
                await outputHandler.WriteFileAsync(outputFile.RelativePath, output);
            }
            else
            {
                outputHandler.CopyFile(file);
            }
        }
    }
}
