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
    public IEnumerable<FilePath> FindFiles(string extension)
    {
        var files = Directory.GetFiles(inputFolder, $"*{extension}", SearchOption.AllDirectories);
        return files.Select(x => FilePath.FromFullPath(x, inputFolder));
    }

    //Returns relative paths to files in the input folder
    public IEnumerable<FilePath> FindFiles()
    {
        var files = Directory.GetFiles(inputFolder, "*.*", SearchOption.AllDirectories);
        return files.Select(x => FilePath.FromFullPath(x, inputFolder));
    }

    public async Task<string> ReadFileAsync(string relativePath)
    {
        var path = Path.Combine(inputFolder, relativePath);
        return await File.ReadAllTextAsync(path);
    }

    public async Task<string> ReadFileAsync(FilePath path)
    {
        return await File.ReadAllTextAsync(path.AbsolutePath);
    }
}
