﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cottle;
using Markdig;
using Markdig.Syntax;
using Ssg_Dotnet.Config;
using Ssg_Dotnet.Files;
using Ssg_Dotnet.LayoutTemplating;
using Ssg_Dotnet.WikiLinks;

namespace Ssg_Dotnet.Processor;

internal class FileProcessor
{
    private readonly InputFileHandler inputHandler;
    private readonly OutputFileHandler outputHandler;
    private readonly InputFileHandler notesInputHandler;
    private readonly OutputFileHandler notesOutputHandler;
    private readonly TemplateHandler contentTemplateHandler;
    private readonly TemplateHandler noteTemplateHandler;

    public FileProcessor(IConfig config)
    {
        inputHandler = new InputFileHandler(config.InputFolder);
        outputHandler = new OutputFileHandler(config.InputFolder, config.OutputFolder);
        notesInputHandler = new InputFileHandler(config.NoteFolder);
        notesOutputHandler = new OutputFileHandler(config.NoteFolder, Path.Combine(config.OutputFolder, "Notes"));
        contentTemplateHandler = new TemplateHandler(config.ContentTemplatePath);
        noteTemplateHandler = new TemplateHandler(config.NoteTemplatePath);
    }

    public async Task ProcessFiles()
    {
        await ProcessFolder(inputHandler, outputHandler, contentTemplateHandler, new Dictionary<string, string>());
        var notes = PreProcessNotes(notesInputHandler);
        await ProcessNotes(notesInputHandler, notesOutputHandler, noteTemplateHandler, new Dictionary<string, string>());
    }

    private static async Task ProcessFolder(InputFileHandler inputHandler, OutputFileHandler outputHandler, TemplateHandler templateHandler, Dictionary<string, string> context)
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
                var cottleValues = new Dictionary<string, string>(context) { { "content", content } };
                var output = await templateHandler.RenderAsync(cottleValues);
                await outputHandler.WriteFileAsync(outputFile.RelativePath, output);
            }
            else
            {
                outputHandler.CopyFile(file);
            }
        }
    }

    private static async Task ProcessNotes(InputFileHandler inputHandler, OutputFileHandler outputHandler, TemplateHandler templateHandler, Dictionary<string, string> context)
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
                var cottleValues = new Dictionary<string, string>(context) { { "content", content } };
                var output = await templateHandler.RenderAsync(cottleValues);
                await outputHandler.WriteFileAsync(outputFile.RelativePath, output);
            }
            else
            {
                outputHandler.CopyFile(file);
            }
        }
    }

    private async Task<Dictionary<string, List<string>>> PreProcessNotes(InputFileHandler notesInputHandler)
    {
        var notes = new HashSet<string>();
        var links = new Dictionary<string, List<string>>(); //key: target, value: origins
        foreach (var file in inputHandler.FindFiles(".md"))
        {
            var filePath = FilePath.FromString(file);
            var note = filePath.RelativeUrl;
            notes.Add(note);
            var input = await inputHandler.ReadFileAsync(file);
            var content = Markdown.Parse(input);
            foreach (var link in content.Descendants().OfType<WikiLink>())
            {
                var target = link.Url!;
                if (!links.ContainsKey(target))
                {
                    links.Add(target, new List<string>());
                }
                links[target].Add(note);
            }
        }
        return links.Where(x => notes.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
    }
}
