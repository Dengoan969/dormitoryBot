using Ninject;
using NUnit.Framework;

namespace TestProject;

public class Tests
{
    private StandardKernel container => new();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var c = container;
        Assert.True(true);
    }
}