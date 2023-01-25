using Ssg_Dotnet.Config;
using Ssg_Dotnet.Processor;
using Ssg_Dotnet.Test.FileSystemReliantTests;

namespace Ssg_Dotnet.Test.ProcessorTests;

[TestFixture, Parallelizable(ParallelScope.Self)]
internal class SampleTests
{
    private readonly string inputFolder = Path.Combine("TestSamples", "Input", "Content");
    private readonly string contentTemplatePath = Path.Combine("TestSamples", "Input", "Layouts", "default.Html");
    private readonly string notesFolder = Path.Combine("TestSamples", "Input", "Notes");
    private readonly string noteTemplatePath = Path.Combine("TestSamples", "Input", "Layouts", "note.Html");
    private const string OutputFolder = "TestOutput";
    private FileProcessor sut;

    [SetUp]
    public void SetUp()
    {
        var config = new ConfigRecord(inputFolder, OutputFolder, notesFolder, contentTemplatePath, noteTemplatePath);
        sut = new FileProcessor(config);
    }

    [Test]
    public async Task ShouldProcessContentFiles()
    {
        // Act
        await sut.ProcessFiles();

        // Assert
        // Find output from unnested files
        var outputFiles = Directory.GetFiles(OutputFolder, "*.*", SearchOption.AllDirectories).Where(x => !x.Contains("Blog") && !x.Contains("Notes"));

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
        await sut.ProcessFiles();

        // Assert
        var outputFiles = Directory.GetFiles(OutputFolder, Path.Combine("Blog", "*.*"), SearchOption.AllDirectories);
        outputFiles.Should().HaveCount(1);
        var outputFile = outputFiles.Single();
        outputFile.Should().EndWith(Path.Combine("Blog", "post1", "index.html")); //Layered into post1/index.html
        File.ReadAllText(outputFile).Should().Be("<h1>Some post</h1>\n");
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(OutputFolder))
        {
            Directory.Delete(OutputFolder, true);
        }
    }

    [Test]
    public async Task ShouldProcessNoteFiles()
    {
        // Act
        await sut.ProcessFiles();

        // Assert
        // Find output from unnested files
        var outputFiles = Directory.GetFiles(OutputFolder, "*.*", SearchOption.AllDirectories).Where(x => x.Contains("Notes"));

        outputFiles.Should().HaveCount(2);
        var expected1 = Path.Combine("Notes", "git", "index.html");
        var expected2 = Path.Combine("Notes", "work-tools", "index.html");
        outputFiles.Should().Contain(x => x.EndsWith(expected1));
        outputFiles.Should().Contain(x => x.EndsWith(expected2));
        //Files contain content
        File.ReadAllText(outputFiles.First(x => x.EndsWith(expected1))).Should().Be("<h1>Git</h1>\n<p>Popular version control tool. See more here <a href=\"https://git-scm.com/\">git</a></p>\n, ");
        File.ReadAllText(outputFiles.First(x => x.EndsWith(expected2))).Should().Be("<h1>Work tools</h1>\n<p>Some work tools include:</p>\n<ul>\n<li>[[git]]</li>\n<li>Ssdt</li>\n</ul>\n, ");
    }
}
