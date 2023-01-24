using System.IO;

namespace Ssg_Dotnet.Files;

internal record FilePath(string DirectoryPath, string FileName, string Extension)
{
    public static FilePath FromString(string relativePath)
    {
        return new FilePath(
            DirectoryPath: Path.GetDirectoryName(relativePath)!,
            FileName: Path.GetFileNameWithoutExtension(relativePath)!,
            Extension: Path.GetExtension(relativePath).ToLower()!
        );
    }

    public string FileNameWithExtension => FileName + Extension;
    public string RelativePath => Path.Combine(DirectoryPath, FileNameWithExtension);
    public string RelativeUrl => Path.Combine(DirectoryPath, FileName);

    public FilePath ToIndexHtml()
    {
        if (FileName == "index")
        {
            return this with { Extension = ".html" };
        }
        return new(DirectoryPath: Path.Combine(DirectoryPath, FileName), FileName: "index", Extension: ".html");
    }
}
