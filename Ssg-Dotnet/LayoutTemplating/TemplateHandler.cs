using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cottle;
using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.LayoutTemplating;

internal class TemplateHandler
{
    private readonly Dictionary<string, IDocument> templates = new();

    public async Task<Dictionary<string, IDocument>> PrepareLayouts(string? layoutfolder)
    {
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
                var document = GetDocumentTrimNothing(template);
                templates.Add(file.FileName, document);
            }
        }
        else
        {
            //Use default layout
            const string template = @"<!DOCTYPE html><html><head><meta charset=""utf-8"" /><title>{title}</title></head><body>{content}</body></html>";
            var document = GetDocumentTrimNothing(template);
            templates.Add("default", document);
        }
        return templates;
    }

    public string Render(string templateName, string title, string content)
    {
        return templates[templateName].Render(Context.CreateBuiltin(new Dictionary<Value, Value>
        {
            ["content"] = content,
            ["title"] = title
        }));
    }

    private static IDocument GetDocumentTrimNothing(string template)
    {
        var configuration = new DocumentConfiguration { Trimmer = DocumentConfiguration.TrimNothing };
        return Document.CreateDefault(template, configuration).DocumentOrThrow;
    }
}
