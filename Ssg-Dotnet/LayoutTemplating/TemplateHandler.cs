using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cottle;
using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.LayoutTemplating;

internal static class TemplateHandler
{
    public static async Task<Dictionary<string, IDocument>> PrepareLayouts(string? layoutfolder)
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
