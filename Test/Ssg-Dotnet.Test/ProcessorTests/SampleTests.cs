using Ssg_Dotnet.Config;
using Ssg_Dotnet.Processor;

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

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(OutputFolder))
        {
            Directory.Delete(OutputFolder, true);
        }
    }

    [Test]
    public async Task ShouldProcessAll()
    {
        // Act
        await sut.ProcessFiles();

        var outputFiles = Directory.GetFiles(OutputFolder, "*.*", SearchOption.AllDirectories);
        foreach (var outputPath in outputFiles)
        {
            var relativePath = Path.GetRelativePath(OutputFolder, outputPath);
            var expectedPath = Path.Combine("TestSamples", "Expected", relativePath);
            File.Exists(expectedPath).Should().BeTrue();
            using var actualFile = File.OpenText(outputPath);
            using var expectedFile = File.OpenText(expectedPath);
            //For each line in each file assert that they match expected
            //This gives better output in case of faillure than just comparing the whole file
            //Unfortunately it does not compare linebreaks
            string? actualLine;
            while ((actualLine = actualFile.ReadLine()) is not null)
            {
                var expectedLine = expectedFile.ReadLine();
                expectedLine.Should().NotBeNull();
                actualLine.Should().Be(expectedLine);
            }
        }
    }
}
