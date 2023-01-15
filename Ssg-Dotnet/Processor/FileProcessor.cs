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
        var filesToProcess = FileFinder.FindFiles(inputFolder);
        foreach(var file in filesToProcess)
        {
            if (file.Extension == ".md")
            {
                var input = await FileHandler.ReadFileAsync(file);
                var output = Markdown.ToHtml(input.Content);
                var outputFilePath = Path.Combine(file.DirectoryPath.Replace(inputFolder, outputFolder), file.FileName + ".html");
                await FileHandler.WriteFileAsync(outputFilePath, output);
            }
            else
            {
                FileHandler.CopyFile(file, outputFolder);
            }
        }
    }
}
