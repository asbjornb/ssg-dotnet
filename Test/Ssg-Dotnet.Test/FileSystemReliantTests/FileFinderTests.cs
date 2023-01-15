using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture]
public class FileFinderTests
{
    [SetUp]
    public void SetUp()
    {
        FileSystemSetup.CreateFiles();
    }

    [TearDown]
    public void TearDown()
    {
        FileSystemSetup.CleanUp();
    }

    [Test]
    public void ShouldFindAllFiles()
    {
        var files = FileFinder.FindFiles(FileSystemSetup.FolderName);
        files.Should().HaveCount(FileSystemSetup.Files.Count);
        foreach (var file in FileSystemSetup.Files)
        {
            files.Select(x => x.FileName + x.Extension).Should().Contain(file);
        }
    }

    [Test]
    public void ShouldFindFilesWithExtension()
    {
        var files = FileFinder.FindFiles(FileSystemSetup.FolderName, "md");
        var expected = FileSystemSetup.Files.Where(x => x.EndsWith(".md")).ToList();
        files.Should().HaveCount(expected.Count);
        foreach (var file in expected)
        {
            files.Select(x => x.FileName + x.Extension).Should().Contain(file);
        }
    }
}
