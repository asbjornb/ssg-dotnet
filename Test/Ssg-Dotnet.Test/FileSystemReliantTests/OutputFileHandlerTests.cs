using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture, Parallelizable(ParallelScope.Self)]
public class OutputFileHandlerTests
{
    private const string FileName = "now.md";
    private const string Content = "Some content";
    private FileSystemHelper inputHelper;
    private FileSystemHelper outputHelper;
    private OutputFileHandler sut;

    [SetUp]
    public async Task SetUp()
    {
        inputHelper = new FileSystemHelper();
        outputHelper = new FileSystemHelper();
        await inputHelper.CreateFileWithContent(FileName, Content);
        sut = new OutputFileHandler(inputHelper.FolderName, outputHelper.FolderName);
    }

    [TearDown]
    public void TearDown()
    {
        inputHelper.Dispose();
        outputHelper.Dispose();
    }

    [Test]
    public async Task ShouldWriteContent()
    {
        const string fileName = "now2.md";
        const string content = "content^2";
        await sut.WriteFileAsync(fileName, content);
        var filePath = Path.Combine(outputHelper.FolderName, fileName);

        File.Exists(filePath).Should().BeTrue();
        var result = await File.ReadAllTextAsync(filePath);
        result.Should().Be(content);
    }

    [Test]
    public async Task ShouldCopyFile()
    {
        //var newFolder = Path.Combine(filePath.DirectoryPath, "SubFolder");
        sut.CopyFile(FileName);
        var filePath = Path.Combine(outputHelper.FolderName, FileName);

        File.Exists(filePath).Should().BeTrue();
        var result = await File.ReadAllTextAsync(filePath);
        result.Should().Be(Content);
    }

    [Test]
    public async Task ShouldCopyFileWithNewName()
    {
        const string newFileName = "now2.md";
        const string newFolderName = "SubFolder";
        sut.CopyFile(FileName, newFolderName, newFileName);

        var path = Path.Combine(outputHelper.FolderName, newFolderName, newFileName);
        File.Exists(path).Should().BeTrue();
        var result = await File.ReadAllTextAsync(path);
        result.Should().Be(Content);
    }
}
