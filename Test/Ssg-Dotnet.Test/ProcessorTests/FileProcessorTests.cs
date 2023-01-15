using Ssg_Dotnet.Processor;

namespace Ssg_Dotnet.Test.ProcessorTests;
internal class FileProcessorTests
{
    private const string InputFolder = "TestSamples";
    private const string OutputFolder = "TestOutput";

    [Test]
    public async Task ShouldProcessFiles()
    {
        // Act
        await FileProcessor.ProcessFiles(InputFolder, OutputFolder);

        // Assert
        var outputFiles = Directory.GetFiles(OutputFolder);
        outputFiles.Should().HaveCount(3);
        outputFiles.Should().Contain(x => x.EndsWith("index.html"));
        outputFiles.Should().Contain(x => x.EndsWith("now.html"));
        outputFiles.Should().Contain(x => x.EndsWith("passthrough.html"));
        File.ReadAllText(outputFiles.First(x => x.EndsWith("index.html"))).Should().Be("<h1>Some header</h1>\n");
        File.ReadAllText(outputFiles.First(x => x.EndsWith("now.html"))).Should().Be("<h1>What I'm doing now</h1>\n<ul>\n<li>Some list point 1</li>\n<li>Point 2</li>\n</ul>\n");
        File.ReadAllText(outputFiles.First(x => x.EndsWith("passthrough.html"))).Should().Be("<head><title>TestTitle</title></head>\r\n<body>\r\n<p>Hello</p>\r\n</body>\r\n");
    }

    [Test]
    public async Task ShouldProcessNestedFiles()
    {
        // Act
        await FileProcessor.ProcessFiles(InputFolder, OutputFolder);

        // Assert
        var outputFiles = Directory.GetFiles(OutputFolder, "Blog\\*.*", SearchOption.AllDirectories);
        outputFiles.Should().HaveCount(1);
        var outputFile = outputFiles.Single();
        outputFile.Should().EndWith("Blog\\post1.html");
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
}
