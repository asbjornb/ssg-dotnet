using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

//Small wrapper collecting the functionality I need from System.File, System.Directory and System.Path
namespace Ssg_Dotnet.Files;
internal class InputFileHandler
{
    private readonly string inputFolder;

    public InputFileHandler(string inputFolder)
    {
        this.inputFolder = inputFolder;
    }

    //Returns relative paths to files in the input folder
    public IEnumerable<string> FindFiles(string extension)
    {
        var files = Directory.GetFiles(inputFolder, $"*{extension}", SearchOption.AllDirectories);
        return files.Select(x => Path.GetRelativePath(inputFolder, x));
    }

    //Returns relative paths to files in the input folder
    public IEnumerable<string> FindFiles()
    {
        var files = Directory.GetFiles(inputFolder, "*.*", SearchOption.AllDirectories);
        return files.Select(x => Path.GetRelativePath(inputFolder, x));
    }

    public async Task<string> ReadFileAsync(string relativePath)
    {
        var path = Path.Combine(inputFolder, relativePath);
        return await File.ReadAllTextAsync(path);
    }
}
