using Markdig;
using Ssg_Dotnet.Config;
using Ssg_Dotnet.Processor;
using Ssg_Dotnet.Test.FileSystemReliantTests;
using Ssg_Dotnet.WikiLinks;

namespace Ssg_Dotnet.Test.ProcessorTests;

[TestFixture, Parallelizable(ParallelScope.Self)]
internal class FileProcessorTests
{
    private FileSystemHelper outputHelper;

    [SetUp]
    public void SetUp()
    {
        outputHelper = new FileSystemHelper();
    }

    [Test]
    public async Task ShouldProcessWithLayout()
    {
        //Arrange
        using var layoutHelper = new FileSystemHelper();
        await layoutHelper.CreateFileWithContent("default.html", "<html><head><title>TestTitle</title></head><body>{content}</body></html>");
        var templatePath = Path.Combine(layoutHelper.FolderName, "default.html");

        using var inputHelper = new FileSystemHelper();
        await inputHelper.CreateFileWithContent("index.md", "# Some header");

        using var notesHelper = new FileSystemHelper(); //Empty notes folder

        var config = new ConfigRecord(inputHelper.FolderName, outputHelper.FolderName, notesHelper.FolderName, templatePath, templatePath);
        var sut = new FileProcessor(config, new MarkdownPipelineBuilder().UseWikiLinks().Build());

        // Act
        await sut.ProcessFiles();

        // Assert
        var outputFiles = Directory.GetFiles(outputHelper.FolderName, "*.*", SearchOption.AllDirectories);
        outputFiles.Should().HaveCount(1);
        var outputFile = outputFiles.Single();
        outputFile.Should().EndWith("index.html");
        File.ReadAllText(outputFile).Should().Be("<html><head><title>TestTitle</title></head><body><h1>Some header</h1>\n</body></html>");
    }

    [Test]
    public async Task ShouldProcessNotes()
    {
        //Arrange
        using var layoutHelper = new FileSystemHelper();
        await layoutHelper.CreateFileWithContent("default.html", "{content},{backlinks}");
        var templatePath = Path.Combine(layoutHelper.FolderName, "default.html");

        using var inputHelper = new FileSystemHelper();

        using var notesHelper = new FileSystemHelper();
        await notesHelper.CreateFileWithContent("note1.md", "# Some header");
        await notesHelper.CreateFileWithContent("note2.md", "# Some header 2\n\n[note1](note1.md)");

        var config = new ConfigRecord(inputHelper.FolderName, outputHelper.FolderName, notesHelper.FolderName, templatePath, templatePath);
        var sut = new FileProcessor(config, new MarkdownPipelineBuilder().UseWikiLinks().Build());

        // Act
        await sut.ProcessFiles();

        // Assert
        var outputFiles = Directory.GetFiles(outputHelper.FolderName, "*.*", SearchOption.AllDirectories);
        outputFiles.Should().HaveCount(2);

        var expected1 = Path.Combine("note1", "index.html");
        var expected2 = Path.Combine("note2", "index.html");
        outputFiles.Should().Contain(x => x.EndsWith(expected1));
        outputFiles.Should().Contain(x => x.EndsWith(expected2));
        //Files contain content
        File.ReadAllText(outputFiles.First(x => x.EndsWith(expected1))).Should().Be("<h1>Some header</h1>\n,");
        File.ReadAllText(outputFiles.First(x => x.EndsWith(expected2))).Should().Be("<h1>Some header 2</h1>\n<p><a href=\"note1.md\">note1</a></p>\n,");
    }

    [TearDown]
    public void TearDown()
    {
        outputHelper.Dispose();
    }
}
