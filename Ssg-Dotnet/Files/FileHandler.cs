using System.IO;
using System.Threading.Tasks;

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
        var destinationPath = Path.Combine(destinationFolder, path.FileName + path.Extension);
        File.Copy(path.FullPath, destinationPath);
    }

    public static void DeleteFile(FilePath path)
    {
        File.Delete(path.FullPath);
    }
}
