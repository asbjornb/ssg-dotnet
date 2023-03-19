using NUnit.Framework.Internal;
using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture, Parallelizable(ParallelScope.Self)]
internal class InputFileHandlerTests
{
    private FileSystemHelper helper;
    private InputFileHandler sut;

    [SetUp]
    public void SetUp()
    {
        helper = new FileSystemHelper();
        sut = new InputFileHandler(helper.FolderName);
    }

    [TearDown]
    public void TearDown()
    {
        helper.Dispose();
    }

    [Test]
    public async Task ShouldReadContent()
    {
        //Arrange
        const string FileName = "now.md";
        const string Content = "Some content";
        await helper.CreateFileWithContent(FileName, Content);
        var filePath = FilePath.FromFullPath(Path.Combine(helper.FolderName, FileName), helper.FolderName);

        //Act
        var content = await InputFileHandler.ReadFileAsync(filePath);

        //Assert
        content.Should().Be(Content);
    }

    [Test]
    public async Task ShouldFindAllFiles()
    {
        var testFiles = new List<string>() { "testFile1.txt", "testFile2.md", "testFile3.json" };
        await helper.CreateFiles(testFiles);

        var files = sut.FindFiles();
        files.Should().HaveCount(testFiles.Count);
        foreach (var file in testFiles)
        {
            files.Select(x => x.RelativePath).Should().Contain(file);
        }
    }

    [Test]
    public async Task ShouldFindFilesWithExtension()
    {
        var testFiles = new List<string>() { "testFile1.txt", "testFile2.md", "testFile3.json" };
        await helper.CreateFiles(testFiles);

        var files = sut.FindFiles("md");
        var expected = testFiles.Where(x => x.EndsWith(".md")).ToList();
        files.Should().HaveCount(expected.Count);
        foreach (var file in expected)
        {
            files.Select(x => x.RelativePath).Should().Contain(file);
        }
    }
}
