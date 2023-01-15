using FluentAssertions;
using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture]
public class FilePathTests
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
    public void ShouldCreatePath()
    {
        var filePath = FilePath.FromFullPath(fullPath);
        filePath.Should().NotBeNull();
        filePath.FullPath.Should().EndWith(FileName);
        filePath.DirectoryPath.Should().NotBeNull();
        filePath.FileName.Should().NotBeNull();
        filePath.Extension.Should().NotBeNull();
    }

    [Test]
    public async Task ShouldReadContent()
    {
        var filePath = FilePath.FromFullPath(fullPath);
        var content = await FileHandler.ReadFileAsync(filePath);
        content.Should().NotBeNull();
        content.Content.Should().Be(Content);
    }

    [Test]
    public void ShouldDeleteFile()
    {
        var filePath = FilePath.FromFullPath(fullPath);
        FileHandler.DeleteFile(filePath);
        File.Exists(filePath.FullPath).Should().BeFalse();
    }
}
