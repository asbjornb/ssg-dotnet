namespace ssg_dotnet.Files;

internal class InputFile
{
    public string Path { get; }
    public string FileName { get; }
    public string Content { get; }
    public string Extension { get; }

    public InputFile(string path, string fileName, string content, string extension)
    {
        Path = path;
        FileName = fileName;
        Content = content;
        Extension = extension;
    }
}
