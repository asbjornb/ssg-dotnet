using System.IO;
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

    public async Task<string> ReadFileAsync(string relativePath)
    {
        var path = Path.Combine(inputFolder, relativePath);
        return await File.ReadAllTextAsync(path);
    }
}
