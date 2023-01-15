using System.IO;
using System.Threading.Tasks;

namespace Ssg_Dotnet.Files;
internal static class FileHandler
{
    public static async Task<string> ReadFileAsync(FilePath path)
    {
        return await File.ReadAllTextAsync(path.FullPath);
    }

    public static async Task WriteFileAsync(string filepath, string content)
    {
        await File.WriteAllTextAsync(filepath, content);
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
