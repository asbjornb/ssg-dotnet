using System.IO;

namespace Ssg_Dotnet.Files;

internal record FilePath(string DirectoryPath, string FileName, string Extension)
{
    public static FilePath FromFullPath(string fullPath)
    {
        return new FilePath(
            DirectoryPath: Path.GetDirectoryName(fullPath)!,
            FileName: Path.GetFileNameWithoutExtension(fullPath)!,
            Extension: Path.GetExtension(fullPath).ToLower()!
        );
    }

    public string FileNameWithExtension => FileName + Extension;

    public string FullPath => Path.Combine(DirectoryPath, FileNameWithExtension);
}
