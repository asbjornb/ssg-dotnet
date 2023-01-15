using System;
using System.IO;

namespace Ssg_Dotnet.Files;

internal record FilePath(string DirectoryPath, string FileName, string Extension)
{
    public static FilePath FromFullPath(string fullPath)
    {
        if (!File.Exists(fullPath))
        {
            throw new ArgumentException("File doesn't exist");
        }
        return new FilePath(
            DirectoryPath: Path.GetDirectoryName(fullPath)!,
            FileName: Path.GetFileNameWithoutExtension(fullPath)!,
            Extension: Path.GetExtension(fullPath)!
        );
    }

    public string FullPath => Path.Combine(DirectoryPath, FileName + Extension);
}
