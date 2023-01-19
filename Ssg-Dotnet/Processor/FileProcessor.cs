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
        if (!Directory.Exists(inputFolder))
        {
            throw new ArgumentException("inputFolder doesn't exist");
        }
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }
        foreach (var file in FileFinder.FindFiles(inputFolder))
        {
            if (file.Extension == ".md")
            {
                var input = await FileHandler.ReadFileAsync(file);
                var content = Markdown.ToHtml(input);
                var outputDirectory = file.DirectoryPath.Replace(inputFolder, outputFolder);
                var outputFile = file! with { DirectoryPath = outputDirectory, Extension = ".html" };
                var output = templateHandler.Render("default", file.FileName, content);
                await FileHandler.WriteFileAsync(outputFile, output);
            }
            else
            {
                FileHandler.CopyFile(file, file.DirectoryPath.Replace(inputFolder, outputFolder));
            }
        }
    }
}
