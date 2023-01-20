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

    public async Task<string> RenderAsync(Dictionary<string, string> content)
    {
        if (template == null)
        {
            await PrepareLayout();
        }
        var values = new Dictionary<Value, Value>();
        foreach (var item in content)
        {
            values.Add(item.Key, item.Value);
        }
        var context = Context.CreateBuiltin(values);
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
