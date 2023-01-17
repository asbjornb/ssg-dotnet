using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cottle;
using Markdig;
using Ssg_Dotnet.Files;
using Ssg_Dotnet.LayoutTemplating;

namespace Ssg_Dotnet.Processor;

internal static class FileProcessor
{
    public static async Task ProcessFiles(string inputFolder, string outputFolder, string? layoutfolder)
    {
        if (!Directory.Exists(inputFolder))
        {
            throw new ArgumentException("inputFolder doesn't exist");
        }
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }
        var layouts = await TemplateHandler.PrepareLayouts(layoutfolder);
        foreach (var file in FileFinder.FindFiles(inputFolder))
        {
            if (file.Extension == ".md")
            {
                var input = await FileHandler.ReadFileAsync(file);
                var content = Markdown.ToHtml(input);
                var outputDirectory = file.DirectoryPath.Replace(inputFolder, outputFolder);
                var outputFile = file! with { DirectoryPath = outputDirectory, Extension = ".html" };
                var output = layouts["default"].Render(Context.CreateBuiltin(new Dictionary<Value, Value>{["content"] = content}));
                await FileHandler.WriteFileAsync(outputFile, output);
            }
            else
            {
                FileHandler.CopyFile(file, file.DirectoryPath.Replace(inputFolder, outputFolder));
            }
        }
    }
}
