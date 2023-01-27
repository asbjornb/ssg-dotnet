using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cottle;

namespace Ssg_Dotnet.LayoutTemplating;

internal class TemplateHandler
{
    private readonly string templatePath;
    private IDocument? template;

    public TemplateHandler(string templatePath)
    {
        this.templatePath = templatePath;
    }

    public async Task<string> RenderAsync(Dictionary<string, string> context, string content)
    {
        if (template == null)
        {
            await PrepareLayout();
        }
        var values = CreateCottleDict(context, content);
        var input = Context.CreateBuiltin(values);
        return template!.Render(input);
    }

    public static Dictionary<Value, Value> CreateCottleDict(Dictionary<string, string> context, string content)
    {
        var values = new Dictionary<Value, Value>();
        foreach (var item in context)
        {
            values.Add(item.Key, item.Value);
        }
        values.Add("content", content);
        return values;
    }

    public async Task<string> RenderAsync(Dictionary<Value, Value> content)
    {
        if (template == null)
        {
            await PrepareLayout();
        }
        var context = Context.CreateBuiltin(content);
        return template!.Render(context);
    }

    private async Task PrepareLayout()
    {
        var fileContent = await File.ReadAllTextAsync(templatePath);
        template = GetDocumentTrimNothing(fileContent);
    }

    private static IDocument GetDocumentTrimNothing(string template)
    {
        var configuration = new DocumentConfiguration { Trimmer = DocumentConfiguration.TrimNothing };
        return Document.CreateDefault(template, configuration).DocumentOrThrow;
    }
}
