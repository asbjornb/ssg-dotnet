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

    public static void DeleteFile(FilePath path)
    {
        File.Delete(path.FullPath);
    }
}
