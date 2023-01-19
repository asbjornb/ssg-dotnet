using NUnit.Framework.Internal;
using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture, Parallelizable(ParallelScope.Self)]
public class InputFileHandlerTests
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

        //Act
        var content = await sut.ReadFileAsync(FileName);

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
            files.Should().Contain(file);
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
            files.Should().Contain(file);
        }
    }
}
