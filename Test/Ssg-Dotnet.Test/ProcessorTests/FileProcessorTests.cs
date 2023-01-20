using Ssg_Dotnet.Processor;
using Ssg_Dotnet.LayoutTemplating;
using Ssg_Dotnet.Test.FileSystemReliantTests;

namespace Ssg_Dotnet.Test.ProcessorTests;
internal class FileProcessorTests
{
    private readonly string InputFolder = Path.Combine("TestSamples", "Content");
    private readonly string LayoutFolder = Path.Combine("TestSamples", "Layouts");
    private const string OutputFolder = "TestOutput";

    public FileProcessor sut;

    [SetUp]
    public void SetUp()
    {
        sut = new FileProcessor(new TemplateHandler());
    }

    [Test]
    public async Task ShouldProcessFiles()
    {
        // Act
        await sut.ProcessFiles(InputFolder, OutputFolder, null, LayoutFolder);

        // Assert
        // Find output from unnested files
        var outputFiles = Directory.GetFiles(OutputFolder, "*.*", SearchOption.AllDirectories).Where(x => !x.Contains("Blog"));

        outputFiles.Should().HaveCount(3);
        var expected1 = Path.Combine(OutputFolder, "index.html"); //No layering since it's already called index
        var expected2 = Path.Combine("now", "index.html"); //Expected to layer into now folder as index.html
        outputFiles.Should().Contain(expected1);
        outputFiles.Should().Contain(x => x.EndsWith(expected2));
        outputFiles.Should().Contain(x => x.EndsWith("passthrough.html")); //No layering since it's already an html file
        //Files contain content
        File.ReadAllText(expected1).Should().Be("<h1>Some header</h1>\n");
        File.ReadAllText(outputFiles.First(x => x.EndsWith(expected2))).Should().Be("<h1>What I'm doing now</h1>\n<ul>\n<li>Some list point 1</li>\n<li>Point 2</li>\n</ul>\n");
        File.ReadAllText(outputFiles.First(x => x.EndsWith("passthrough.html"))).Should().Be("<head><title>TestTitle</title></head>\r\n<body>\r\n<p>Hello</p>\r\n</body>\r\n");
    }

    [Test]
    public async Task ShouldProcessNestedFiles()
    {
        // Act
        await sut.ProcessFiles(InputFolder, OutputFolder, null, LayoutFolder);

        // Assert
        var outputFiles = Directory.GetFiles(OutputFolder, Path.Combine("Blog", "*.*"), SearchOption.AllDirectories);
        outputFiles.Should().HaveCount(1);
        var outputFile = outputFiles.Single();
        outputFile.Should().EndWith(Path.Combine("Blog", "post1", "index.html")); //Layered into post1/index.html
        File.ReadAllText(outputFile).Should().Be("<h1>Some post</h1>\n");
    }

    [Test]
    public async Task ShouldProcessWithLayout()
    {
        //Arrange
        using var layoutHelper = new FileSystemHelper();
        await layoutHelper.CreateFileWithContent("default.html", "<html><head><title>TestTitle</title></head><body>{content}</body></html>");

        using var inputHelper = new FileSystemHelper();
        await inputHelper.CreateFileWithContent("index.md", "# Some header");

        // Act
        await sut.ProcessFiles(inputHelper.FolderName, OutputFolder, null, layoutHelper.FolderName);

        // Assert
        var outputFiles = Directory.GetFiles(OutputFolder, "*.*", SearchOption.AllDirectories);
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

        using var inputHelper = new FileSystemHelper();

        using var notesHelper = new FileSystemHelper();
        await notesHelper.CreateFileWithContent("note1.md", "# Some header");
        await notesHelper.CreateFileWithContent("note2.md", "# Some header 2\n\n[note1](note1.md)");

        // Act
        await sut.ProcessFiles(inputHelper.FolderName, OutputFolder, notesHelper.FolderName, layoutHelper.FolderName);

        // Assert
        var outputFiles = Directory.GetFiles(OutputFolder, "*.*", SearchOption.AllDirectories);
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
        if (Directory.Exists(OutputFolder))
        {
            Directory.Delete(OutputFolder, true);
        }
    }
}
