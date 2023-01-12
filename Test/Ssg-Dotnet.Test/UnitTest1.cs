namespace Ssg_Dotnet.Test;

[TestFixture, Parallelizable(ParallelScope.Self)]
public class Tests
{
    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}
