namespace Ssg_Dotnet.Test.FileSystemReliantTests;

internal class FileSystemHelper : IDisposable
{
    private readonly DirectoryInfo testDirectory;
    public string FolderName => testDirectory?.FullName ?? throw new InvalidOperationException("Test directory not created");
    public FileSystemHelper()
    {
        testDirectory = Directory.CreateTempSubdirectory("Ssg-Dotnet.Test");
    }

    public async Task CreateFiles(IEnumerable<string> fileNames)
    {
        foreach (var file in fileNames)
        {
            await File.WriteAllTextAsync($"{FolderName}/{file}", "SomeText");
        }
    }

    public async Task CreateFileWithContent(string fileName, string content)
    {
        await File.WriteAllTextAsync($"{FolderName}/{fileName}", content);
    }

    public void Dispose()
    {
        testDirectory?.Delete(true);
    }
}
