using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture, Parallelizable(ParallelScope.Self)]
public class FileFinderTests
{
    private FileSystemHelper helper;
    private readonly List<string> testFiles = new() { "testFile1.txt", "testFile2.md", "testFile3.json" };

    [SetUp]
    public async Task SetUp()
    {
        helper = new FileSystemHelper();
        await helper.CreateFiles(testFiles);
    }

    [TearDown]
    public void TearDown()
    {
        helper.Dispose();
    }

    [Test]
    public void ShouldFindAllFiles()
    {
        var files = FileFinder.FindFiles(helper.FolderName);
        files.Should().HaveCount(testFiles.Count);
        foreach (var file in testFiles)
        {
            files.Select(x => x.FileName + x.Extension).Should().Contain(file);
        }
    }

    [Test]
    public void ShouldFindFilesWithExtension()
    {
        var files = FileFinder.FindFiles(helper.FolderName, "md");
        var expected = testFiles.Where(x => x.EndsWith(".md")).ToList();
        files.Should().HaveCount(expected.Count);
        foreach (var file in expected)
        {
            files.Select(x => x.FileName + x.Extension).Should().Contain(file);
        }
    }
}
