using System.Collections.Generic;
using System.IO;
using System.Text;
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
            var layoutReader = new InputFileHandler(layoutfolder);
            //Use cottle to set up layout
            foreach (var file in layoutReader.FindFiles(".html"))
            {
                var template = await layoutReader.ReadFileAsync(file);
                var document = GetDocumentTrimNothing(template);
                var name = Path.GetFileNameWithoutExtension(file);
                templates.Add(name, document);
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
        //Should be changed eventually to match in priority order original file name, original folder name or any x-parent folder.
        var template = templates[templateName];
        return RenderTemplate(template, title, content);
    }

    public string RenderNote(string templateName, string title, string content, List<string> backlinks)
    {
        var template = templates[templateName];
        return RenderTemplate(template, title, content, backlinks);
    }

    private static string RenderTemplate(IDocument template, string title, string content)
    {
        return template.Render(Context.CreateBuiltin(new Dictionary<Value, Value>
        {
            ["content"] = content,
            ["title"] = title
        }));
    }

    private static string RenderTemplate(IDocument template, string title, string content, List<string> backlinks)
    {
        var dictionary = new Dictionary<Value, Value>
        {
            ["content"] = content,
            ["title"] = title
        };
        if (backlinks.Count > 0)
        {
            var backLinkBuilder = new StringBuilder("<p class=\"backlinks-title larger\">Backlinks</p>");
            foreach (var link in backlinks)
            {
                backLinkBuilder.Append($"<p class=\"backlinks\"><a href=\"{link}\">{link}</a></p>");
            }
            dictionary.Add("backlinks", backLinkBuilder.ToString());
        }
        return template.Render(Context.CreateBuiltin(dictionary));
    }

    private static IDocument GetDocumentTrimNothing(string template)
    {
        var configuration = new DocumentConfiguration { Trimmer = DocumentConfiguration.TrimNothing };
        return Document.CreateDefault(template, configuration).DocumentOrThrow;
    }
}
