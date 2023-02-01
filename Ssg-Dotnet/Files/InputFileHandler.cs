using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

//Small wrapper collecting the functionality I need from System.File, System.Directory and System.Path
namespace Ssg_Dotnet.Files;
internal class InputFileHandler
{
    private readonly List<string> filters = new() { ".vscode", ".git", ".gitignore", ".noncontent" };

    private readonly string inputFolder;

    public InputFileHandler(string inputFolder)
    {
        this.inputFolder = inputFolder;
    }

    //Returns relative paths to files in the input folder
    public IEnumerable<FilePath> FindFiles(string extension)
    {
        var files = Directory.EnumerateFiles(inputFolder, $"*{extension}", SearchOption.AllDirectories);
        var filteredFiles = Filter(files);
        return filteredFiles.Select(x => FilePath.FromFullPath(x, inputFolder));
    }

    //Returns relative paths to files in the input folder
    public IEnumerable<FilePath> FindFiles()
    {
        var files = Directory.EnumerateFiles(inputFolder, "*.*", SearchOption.AllDirectories);
        var filteredFiles = Filter(files);
        return filteredFiles.Select(x => FilePath.FromFullPath(x, inputFolder));
    }

    public static async Task<string> ReadFileAsync(FilePath path)
    {
        return await File.ReadAllTextAsync(path.AbsolutePath);
    }

    private IEnumerable<string> Filter(IEnumerable<string> files)
    {
        //Use hardcoded filter for now - should be able to use .gitignore or other more dynamic filtering at some point
        foreach (var file in files)
        {
            var ignore = false;
            foreach (var filter in filters)
            {
                if (file.Contains(filter))
                {
                    ignore = true;
                    break;
                }
            }
            if (!ignore)
            {
                yield return file;
            }
        }
    }
}
