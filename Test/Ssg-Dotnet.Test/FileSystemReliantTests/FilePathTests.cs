using FluentAssertions;
using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture]
public class FilePathTests
{
    private const string FileName = "now.md";
    private const string Content = "Some content";
    private string fullPath = Path.Combine(FileSystemSetup.FolderName, FileName);

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
        var filePath = new FilePath(fullPath);
        filePath.Should().NotBeNull();
        filePath.FullPath.Should().EndWith(FileName);
        filePath.DirectoryPath.Should().NotBeNull();
        filePath.FileName.Should().NotBeNull();
        filePath.Extension.Should().NotBeNull();
    }

    [Test]
    public async Task ShouldReadContent()
    {
        var filePath = new FilePath(fullPath);
        var content = await filePath.ReadFile();
        content.Should().NotBeNull();
        content.Content.Should().Be(Content);
    }
}
