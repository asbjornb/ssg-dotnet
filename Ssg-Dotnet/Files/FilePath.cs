using System.IO;

namespace Ssg_Dotnet.Files;

internal record FilePath(string DirectoryPath, string FileName, string Extension)
{
    public static FilePath FromString(string fullPath)
    {
        return new FilePath(
            DirectoryPath: Path.GetDirectoryName(fullPath)!,
            FileName: Path.GetFileNameWithoutExtension(fullPath)!,
            Extension: Path.GetExtension(fullPath).ToLower()!
        );
    }

    public string FileNameWithExtension => FileName + Extension;

    public string RelativePath => Path.Combine(DirectoryPath, FileNameWithExtension);

    public FilePath ToIndexHtml()
    {
        if (FileName == "index")
        {
            return this with { Extension = ".html" };
        }
        return new(DirectoryPath: Path.Combine(DirectoryPath, FileName), FileName: "index", Extension: ".html");
    }
}
