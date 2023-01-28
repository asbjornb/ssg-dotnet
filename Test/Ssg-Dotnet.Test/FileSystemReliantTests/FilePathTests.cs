using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture, Parallelizable(ParallelScope.Self)]
internal class FilePathTests
{
    [Test]
    public void ShouldCreatePath()
    {
        const string BaseFolder = "BaseFolder";
        const string SubFolder = "TestSamples";
        const string FileName = "now";
        const string Extension = ".md";
        var fullPath = Path.Combine(BaseFolder, SubFolder, FileName + Extension);
        var filePath = FilePath.FromFullPath(fullPath, BaseFolder);
        filePath.RelativePath.Should().Be(Path.Combine(SubFolder, FileName + Extension));
        filePath.RelativeDir.Should().Be(SubFolder);
        filePath.FileName.Should().Be(FileName);
        filePath.Extension.Should().Be(Extension);
        filePath.FileNameWithExtension.Should().Be(FileName + Extension);
        filePath.RelativeUrl.Should().Be(Path.Combine(SubFolder, FileName));
    }
}
