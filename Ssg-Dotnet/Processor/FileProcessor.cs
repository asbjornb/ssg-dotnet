using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cottle;
using Markdig;
using Ssg_Dotnet.Files;

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
        var layouts = await PrepareLayouts(layoutfolder);
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
                FileHandler.CopyFile(file, outputFolder);
            }
        }
    }

    private static async Task<Dictionary<string, IDocument>> PrepareLayouts(string? layoutfolder)
    {
        var result = new Dictionary<string, IDocument>();
        if (layoutfolder != null)
        {
            if (!Directory.Exists(layoutfolder))
            {
                throw new ArgumentException("layoutfolder doesn't exist");
            }
            //Use cottle to set up layout
            foreach (var file in FileFinder.FindFiles(layoutfolder, ".html"))
            {
                var template = await FileHandler.ReadFileAsync(file);
                var configuration = new DocumentConfiguration { Trimmer = DocumentConfiguration.TrimNothing };
                var document = Document.CreateDefault(template, configuration).DocumentOrThrow;
                result.Add(file.FileName, document);
            }
        }
        else
        {
            //Use default layout
            const string template = @"<!DOCTYPE html><html><head><meta charset=""utf-8"" /><title>{title}</title></head><body>{content}</body></html>";
            var configuration = new DocumentConfiguration { Trimmer = DocumentConfiguration.TrimNothing };
            var document = Document.CreateDefault(template, configuration).DocumentOrThrow;
            result.Add("default", document);
        }
        return result;
    }
}
