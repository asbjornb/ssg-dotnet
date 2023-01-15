using System;
using System.IO;
using System.Threading.Tasks;

namespace Ssg_Dotnet.Files;

internal class FilePath
{
    public string FullPath { get; }
    public string DirectoryPath { get; }
    public string FileName { get; }
    public string Extension { get; }

    public FilePath(string fullPath)
    {
        if (!File.Exists(fullPath))
        {
            throw new ArgumentException("File doesn't exist");
        }
        FullPath = fullPath;
        DirectoryPath = Path.GetDirectoryName(fullPath)!;
        FileName = Path.GetFileNameWithoutExtension(fullPath)!;
        Extension = Path.GetExtension(fullPath)!;
    }

    public async Task<ContentFile> ReadFile()
    {
        var content = await File.ReadAllTextAsync(FullPath);
        return new ContentFile(this, content);
    }

    public void DeleteFile()
    {
        File.Delete(FullPath);
    }
}
