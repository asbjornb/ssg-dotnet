using Cottle;
using FluentAssertions;

namespace Ssg_Dotnet.Test.ExplorationTest;

//Some exploration of the Cottle library to assert that it supports the features wanted
[TestFixture, Parallelizable(ParallelScope.Self)]
public class CottleTests
{
    [Test]
    public void ShouldReplace()
    {
        const string template = "<main>{content}</main>";
        var document = Document.CreateDefault(template).DocumentOrThrow;
        var result = document.Render(Context.CreateBuiltin(new Dictionary<Value, Value>
        {
            ["content"] = "<h1>Hello World</h1>"
        }));
        result.Should().NotBeNull();
        result.Should().Be("<main><h1>Hello World</h1></main>");
    }

    [Test]
    public void ShouldReplaceButKeepWhitespace()
    {
        const string template = "<main>\n        {content}\n    </main>\n";
        var configuration = new DocumentConfiguration { Trimmer = DocumentConfiguration.TrimNothing };
        var document = Document.CreateDefault(template, configuration).DocumentOrThrow;
        var result = document.Render(
            Context.CreateBuiltin(new Dictionary<Value, Value>
            {
                ["content"] = "<h1>Hello World</h1>"
            }));
        result.Should().NotBeNull();
        result.Should().Be("<main>\n        <h1>Hello World</h1>\n    </main>\n");
    }
}
