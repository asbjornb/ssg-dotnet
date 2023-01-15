using System.IO;
using System.Threading.Tasks;

namespace Ssg_Dotnet.Files;
internal static class FileHandler
{
    public static async Task<ContentFile> ReadFileAsync(FilePath path)
    {
        var content = await File.ReadAllTextAsync(path.FullPath);
        return new ContentFile(path, content);
    }

    public static async Task WriteFileAsync(ContentFile file)
    {
        await File.WriteAllTextAsync(file.FilePath.FullPath, file.Content);
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
