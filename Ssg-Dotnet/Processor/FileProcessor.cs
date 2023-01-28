using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using Markdig.Syntax;
using Ssg_Dotnet.Config;
using Ssg_Dotnet.Files;
using Ssg_Dotnet.LayoutTemplating;
using Ssg_Dotnet.Notes;
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
    private readonly MarkdownPipeline pipeline;

    public FileProcessor(IConfig config, MarkdownPipeline pipeline)
    {
        inputHandler = new InputFileHandler(config.InputFolder);
        outputHandler = new OutputFileHandler(config.InputFolder, config.OutputFolder);
        notesInputHandler = new InputFileHandler(config.NoteFolder);
        notesOutputHandler = new OutputFileHandler(config.NoteFolder, Path.Combine(config.OutputFolder, "Notes"));
        contentTemplateHandler = new TemplateHandler(config.ContentTemplatePath);
        noteTemplateHandler = new TemplateHandler(config.NoteTemplatePath);
        this.pipeline = pipeline;
    }

    public async Task ProcessFiles()
    {
        await ProcessFolder(inputHandler, outputHandler, contentTemplateHandler);
        var notes = await PreProcessNotes(notesInputHandler, pipeline);
        await ProcessFolder(notesInputHandler, notesOutputHandler, noteTemplateHandler, new Dictionary<string, string>(), notes);
    }

    private async Task ProcessFolder(InputFileHandler inputHandler, OutputFileHandler outputHandler, TemplateHandler templateHandler)
    {
        var overallContext = new Dictionary<string, string>();
        var individualFileContexts = new Dictionary<string, ICottleEntry>();
        await ProcessFolder(inputHandler, outputHandler, templateHandler, overallContext, individualFileContexts);
    }

    private async Task ProcessFolder(InputFileHandler inputHandler, OutputFileHandler outputHandler, TemplateHandler templateHandler, Dictionary<string, string> overallContext, Dictionary<string, ICottleEntry> individualFileContexts)
    {
        foreach (var file in inputHandler.FindFiles())
        {
            if (file.Extension == ".md")
            {
                var content = await MarkdownFile.ReadFromFile(file, pipeline);
                var cottleValues = new FileContext(overallContext, content.ToHtml());
                if (individualFileContexts.TryGetValue(file.RelativeUrl, out var individualFileContext))
                {
                    cottleValues.AddCottleEntry(individualFileContext);
                }
                var output = await templateHandler.RenderAsync(cottleValues);
                
                //switch extention to .html for outputFile:
                var outputFile = file.ToIndexHtml();
                await outputHandler.WriteFileAsync(outputFile.RelativePath, output);
            }
            else
            {
                outputHandler.CopyFile(file.RelativePath);
            }
        }
    }

    private static async Task<Dictionary<string, ICottleEntry>> PreProcessNotes(InputFileHandler notesInputHandler, MarkdownPipeline pipeline)
    {
        var mdFilePaths = notesInputHandler.FindFiles(".md");
        var mdFiles = await Task.WhenAll(mdFilePaths.Select(async file => await MarkdownFile.ReadFromFile(file, pipeline)));
        var notePreviews = GetNotePreviews(mdFiles); //key: url, value: preview
        var backLinks = new Dictionary<string, List<string>>(); //key: target, value: origins
        foreach (var file in notesInputHandler.FindFiles(".md"))
        {
            var noteUrl = file.RelativeUrl;
            var content = await MarkdownFile.ReadFromFile(file, pipeline);
            var preview = content.GetPreview();
            foreach (var wikiLink in content.Content.Descendants().OfType<WikiLink>())
            {
                var target = wikiLink.Url!;
                if (!backLinks.ContainsKey(target))
                {
                    backLinks.Add(target, new List<string>());
                }
                backLinks[target].Add(noteUrl);
            }
        }
        var result = new Dictionary<string, ICottleEntry>();
        foreach (var link in backLinks)
        {
            if (!result.ContainsKey(link.Key))
            {
                result.Add(link.Key, new NoteLinkCollection());
            }
            foreach (var origin in link.Value)
            {
                ((NoteLinkCollection)result[link.Key]).Add(NoteLink.FromUrl(origin, notePreviews[origin]));
            }
        }
        return result;
    }

    private static Dictionary<string, string> GetNotePreviews(IEnumerable<MarkdownFile> mdFiles)
    {
        var notePreviews = new Dictionary<string, string>();
        foreach(var mdFile in mdFiles)
        {
            notePreviews.Add(mdFile.Path.RelativeUrl, mdFile.GetPreview());
        }
        return notePreviews;
    }
}
