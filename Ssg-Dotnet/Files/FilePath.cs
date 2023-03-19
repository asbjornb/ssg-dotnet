using System.IO;

namespace Ssg_Dotnet.Files;

public sealed class FilePath
{
    private readonly string baseDir;
    public string RelativeDir { get; }
    public string FileName { get; }
    public string Extension { get; }

    private FilePath(string baseDir, string relativeDir, string fileName, string extension)
    {
        this.baseDir = baseDir;
        RelativeDir = relativeDir;
        FileName = fileName;
        Extension = extension;
    }

    public static FilePath FromFullPath(string fullPath, string relativeTo)
    {
        return new FilePath(
            baseDir: relativeTo,
            relativeDir: Path.GetDirectoryName(Path.GetRelativePath(relativeTo, fullPath))!,
            fileName: Path.GetFileNameWithoutExtension(fullPath)!,
            extension: Path.GetExtension(fullPath).ToLower()!
        );
    }

    public string FileNameWithExtension => FileName + Extension;
    public string RelativePath => Path.Combine(RelativeDir, FileNameWithExtension);
    public string RelativeUrl => Path.Combine(RelativeDir, FileName);
    public string AbsolutePath => Path.Combine(baseDir, RelativePath);

    public FilePath ToIndexHtml()
    {
        if (FileName == "index")
        {
            return new(baseDir, RelativeDir, FileName, ".html");
        }
        return new(baseDir, Path.Combine(RelativeDir, FileName), fileName: "index", extension: ".html");
    }
}
