﻿using System.Collections.Generic;
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
        if (notesFolder != null)
        {
            var notesFileHandler = new InputFileHandler(notesFolder);
            var notesOutputHandler = new OutputFileHandler(notesFolder, Path.Combine(outputFolder, "notes"));
            foreach (var file in notesFileHandler.FindFiles())
            {
                var filePath = FilePath.FromString(file);
                if (filePath.Extension == ".md")
                {
                    var input = await notesFileHandler.ReadFileAsync(file);
                    var content = Markdown.ToHtml(input);
                    //switch extention to .html for outputFile:
                    var outputFile = filePath.ToIndexHtml();
                    var output = templateHandler.RenderNote("default", filePath.FileName, content, new List<string>());
                    await notesOutputHandler.WriteFileAsync(outputFile.RelativePath, output);
                }
                else
                {
                    notesOutputHandler.CopyFile(file);
                }
            }
        }

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
