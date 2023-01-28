using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture]
internal class FilePathTests
{
    [Test]
    public void ShouldCreatePath()
    {
        var fullPath = Path.Combine("TestSamples", "now.md");
        var filePath = FilePath.FromString(fullPath);
        filePath.Should().NotBeNull();
        filePath.RelativePath.Should().EndWith(fullPath);
        filePath.DirectoryPath.Should().EndWith("TestSamples");
        filePath.FileName.Should().Be("now");
        filePath.Extension.Should().Be(".md");
    }
}
