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
}
