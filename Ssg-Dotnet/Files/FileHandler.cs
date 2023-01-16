using System.IO;
using System.Threading.Tasks;

//Small wrapper collecting the functionality I need from System.File, System.Directory and System.Path
namespace Ssg_Dotnet.Files;
internal static class FileHandler
{
    public static async Task<string> ReadFileAsync(FilePath path)
    {
        return await File.ReadAllTextAsync(path.FullPath);
    }

    public static async Task WriteFileAsync(FilePath filepath, string content)
    {
        if (!Directory.Exists(filepath.DirectoryPath))
        {
            Directory.CreateDirectory(filepath.DirectoryPath);
        }
        if (File.Exists(filepath.FullPath))
        {
            DeleteFile(filepath);
        }
        await File.WriteAllTextAsync(filepath.FullPath, content);
    }

    public static void CopyFile(FilePath path, string destinationFolder)
    {
        CopyFile(path, destinationFolder, path.FileName + path.Extension);
    }

    public static void CopyFile(FilePath path, string destinationFolder, string destinationFileName)
    {
        if (!Directory.Exists(destinationFolder))
        {
            Directory.CreateDirectory(destinationFolder);
        }
        var destinationPath = Path.Combine(destinationFolder, destinationFileName);
        File.Copy(path.FullPath, destinationPath);
    }

    public static void DeleteFile(FilePath path)
    {
        File.Delete(path.FullPath);
    }
}
