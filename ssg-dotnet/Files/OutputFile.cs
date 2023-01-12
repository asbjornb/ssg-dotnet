namespace ssg_dotnet.Files;

internal class OutputFile
{
    public string Path { get; }
    public string FileName { get; }
    public string Content { get; }
    public string Extension { get; }

    public OutputFile(string path, string fileName, string content, string extension)
    {
        Path = path;
        FileName = fileName;
        Content = content;
        Extension = extension;
    }
}
