using Ssg_Dotnet.Files;
using System.Linq;

namespace Ssg_Dotnet.Test;

[TestFixture, Parallelizable(ParallelScope.Self)]
public class FileFinderTests
{
    [Test]
    public void ShouldFindAllFiles()
    {
        var files = FileFinder.FindFiles("TestSamples/");
        Assert.That(files.ToList(), Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(files.Select(x => x.FileName + x.Extension), Does.Contain("now.md"));
            Assert.That(files.Select(x => x.FileName + x.Extension), Does.Contain("index.md"));
            Assert.That(files.Select(x => x.FileName + x.Extension), Does.Contain("configuration.json"));
        });
    }
}
