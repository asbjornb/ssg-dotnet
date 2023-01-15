using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ssg_Dotnet.Files;
internal static class FileFinder
{
    public static IEnumerable<FilePath> FindFiles(string directoryPath, string extension)
    {
        var files = Directory.GetFiles(directoryPath, $"*{extension}", SearchOption.AllDirectories);
        return files.Select(file => new FilePath(file));
    }

    public static IEnumerable<FilePath> FindFiles(string directoryPath)
    {
        var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
        return files.Select(file => new FilePath(file));
    }
}
