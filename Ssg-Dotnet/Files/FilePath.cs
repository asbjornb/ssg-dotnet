using System;
using System.IO;
using System.Threading.Tasks;

namespace Ssg_Dotnet.Files;

internal record FilePath(string FullPath, string DirectoryPath, string FileName, string Extension)
{
    public static FilePath FromFullPath(string fullPath)
    {
        if (!File.Exists(fullPath))
        {
            throw new ArgumentException("File doesn't exist");
        }
        return new FilePath(
            FullPath: fullPath,
            DirectoryPath: Path.GetDirectoryName(fullPath)!,
            FileName: Path.GetFileNameWithoutExtension(fullPath)!,
            Extension: Path.GetExtension(fullPath)!
        );
    }
}
