using DormitoryBot.Domain.Schedule;
using Ninject;
using NUnit.Framework;

namespace TestProject;

public class Tests
{
    private StandardKernel Container => new();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var c = Container;
        Container.Bind<Schedule>().ToSelf().WithConstructorArgument("db");
        Assert.True(true);
    }
}