using System;
using System.IO;
using System.Threading.Tasks;
using Markdig;
using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Processor;

internal static class FileProcessor
{
    public static async Task ProcessFiles(string inputFolder, string outputFolder)
    {
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
                var output = Markdown.ToHtml(input);
                var outputDirectory = file.DirectoryPath.Replace(inputFolder, outputFolder);
                var outputFile = file! with { DirectoryPath = outputDirectory, Extension = ".html" };
                await FileHandler.WriteFileAsync(outputFile, output);
            }
            else
            {
                FileHandler.CopyFile(file, outputFolder);
            }
        }
    }
}
