using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture, Parallelizable(ParallelScope.Self)]
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
        Assert.That(files.ToList(), Has.Count.EqualTo(FileSystemSetup.Files.Count));
        foreach(var file in FileSystemSetup.Files)
        {
            Assert.That(files.Select(x => x.FileName + x.Extension), Does.Contain(file));
        }
    }

    [Test]
    public void ShouldFindFilesWithExtension()
    {
        var files = FileFinder.FindFiles(FileSystemSetup.FolderName, "md");
        var expected = FileSystemSetup.Files.Where(x => x.EndsWith(".md")).ToList();
        Assert.That(files.ToList(), Has.Count.EqualTo(expected.Count));
        foreach (var file in expected)
        {
            Assert.That(files.Select(x => x.FileName + x.Extension), Does.Contain(file));
        }
    }
}
