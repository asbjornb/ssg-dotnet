using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture]
public class FilePathTests
{
    private const string FileName = "now.md";
    private readonly string fullPath = Path.Combine(FileSystemSetup.FolderName, FileName);

    [Test]
    public void ShouldCreatePath()
    {
        var filePath = FilePath.FromFullPath(fullPath);
        filePath.Should().NotBeNull();
        filePath.FullPath.Should().EndWith(fullPath);
        filePath.DirectoryPath.Should().EndWith(FileSystemSetup.FolderName);
        filePath.FileName.Should().Be("now");
        filePath.Extension.Should().Be(".md");
    }
}
