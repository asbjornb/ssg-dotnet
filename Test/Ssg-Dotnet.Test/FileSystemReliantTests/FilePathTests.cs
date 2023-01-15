using Ssg_Dotnet.Files;

namespace Ssg_Dotnet.Test.FileSystemReliantTests;

[TestFixture]
public class FilePathTests
{
    private const string FileName = "now.md";
    private const string Content = "Some content";
    private string fullPath = Path.Combine(FileSystemSetup.FolderName, FileName);

    [SetUp]
    public void SetUp()
    {
        FileSystemSetup.CreateFile(FileName, Content);
    }

    [TearDown]
    public void TearDown()
    {
        FileSystemSetup.CleanUp();
    }

    [Test]
    public void ShouldCreatePath()
    {
        var filePath = new FilePath(fullPath);
        Assert.That(filePath, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(filePath.FullPath, Does.EndWith(FileName));
            Assert.That(filePath.DirectoryPath, Is.Not.Null);
            Assert.That(filePath.FileName, Is.Not.Null);
            Assert.That(filePath.Extension, Is.Not.Null);
        });
    }

    [Test]
    public async Task ShouldReadContent()
    {
        var filePath = new FilePath(fullPath);
        var content = await filePath.ReadFile();
        Assert.That(content, Is.Not.Null);
        Assert.That(content.Content, Is.EqualTo(Content));
    }
}
