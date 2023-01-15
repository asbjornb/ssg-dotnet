namespace Ssg_Dotnet.Files;

internal class ContentFile
{
    public FilePath FilePath { get; }
    public string Content { get; }

    internal ContentFile(FilePath filePath, string content)
    {
        FilePath = filePath;
        Content = content;
    }
}
