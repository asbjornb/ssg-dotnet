namespace Ssg_Dotnet.Files;

internal abstract class ContentFile
{
    public string Path { get; }
    public string FileName { get; }
    public string Content { get; }
    public string Extension { get; }

    protected ContentFile(string path, string fileName, string content, string extension)
    {
        Path = path;
        FileName = fileName;
        Content = content;
        Extension = extension;
    }
}
