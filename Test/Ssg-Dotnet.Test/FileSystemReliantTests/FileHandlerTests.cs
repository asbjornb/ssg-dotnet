using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture]
public class FileHandlerTests
{
    private const string FileName = "now.md";
    private const string Content = "Some content";
    private readonly string fullPath = Path.Combine(FileSystemSetup.FolderName, FileName);

    [SetUp]
    public void SetUp()
    {
        FileSystemSetup.CreateFile(FileName, Content);
    }

    [TearDown]
    public void TearDown()
    {
        FileSystemSetup.CleanUp();
    }

    [Test]
    public async Task ShouldReadContent()
    {
        var filePath = FilePath.FromFullPath(fullPath);
        var content = await FileHandler.ReadFileAsync(filePath);
        content.Should().Be(Content);
    }

    [Test]
    public void ShouldDeleteFile()
    {
        var filePath = FilePath.FromFullPath(fullPath);
        FileHandler.DeleteFile(filePath);
        File.Exists(filePath.FullPath).Should().BeFalse();
    }

    [Test]
    public async Task ShouldWriteContent()
    {
        const string content = "content^2";
        var filePath = FilePath.FromFullPath(fullPath) with { FileName = "now2" };
        await FileHandler.WriteFileAsync(filePath, content);

        File.Exists(filePath.FullPath).Should().BeTrue();
        var result = await File.ReadAllTextAsync(filePath.FullPath);
        result.Should().Be(content);
    }

    [Test]
    public async Task ShouldCopyFile()
    {
        var filePath = FilePath.FromFullPath(fullPath);
        var newFolder = Path.Combine(filePath.DirectoryPath, "SubFolder");
        FileHandler.CopyFile(filePath, newFolder);
        var newFile = Path.Combine(newFolder, FileName);

        File.Exists(newFile).Should().BeTrue();
        var result = await File.ReadAllTextAsync(newFile);
        result.Should().Be(Content);
    }

    [Test]
    public async Task ShouldCopyFileWithNewName()
    {
        const string newFileName = "now2.md";
        var filePath = FilePath.FromFullPath(fullPath);
        FileHandler.CopyFile(filePath, filePath.DirectoryPath, newFileName);
        var path = Path.Combine(filePath.DirectoryPath, newFileName);

        File.Exists(path).Should().BeTrue();
        var result = await File.ReadAllTextAsync(path);
        result.Should().Be(Content);
    }
}
