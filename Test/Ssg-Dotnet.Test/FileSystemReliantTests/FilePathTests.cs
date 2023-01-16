using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture]
public class FilePathTests
{
    [Test]
    public void ShouldCreatePath()
    {
        var fullPath = Path.Combine("TestSamples", "now.md");
        var filePath = FilePath.FromFullPath(fullPath);
        filePath.Should().NotBeNull();
        filePath.FullPath.Should().EndWith(fullPath);
        filePath.DirectoryPath.Should().EndWith("TestSamples");
        filePath.FileName.Should().Be("now");
        filePath.Extension.Should().Be(".md");
    }
}
