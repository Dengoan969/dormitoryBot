using System;
using System.Collections.Generic;
using System.Linq;
using DomitoryBot.Domain.Schedule;
using DormitoryBot.Domain;
using DormitoryBot.Domain.Schedule;
using Ninject;
using NUnit.Framework;

namespace TestProject;

public class ScheduleTests
{
    private StandardKernel Container => new();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSchedule_WhenAllOk()
    {
        var container = Container;
        container.Bind<IRecordsRepository>()
            .To<MockScheduleRepository>()
            .WithConstructorArgument("freeTimes", new Dictionary<string, bool[]>
            {
                {"1", new bool[48 * 3]},
                {"2", new bool[48 * 3]},
                {"3", new bool[48 * 3]}
            })
            .WithConstructorArgument("dataBase", new Dictionary<long, List<ScheduleRecord>>());

        container.Bind<Schedule>().ToSelf().WithConstructorArgument("washingTypes",
            new Dictionary<string, TimeSpan>
            {
                {"Полчаса", TimeSpan.FromMinutes(30)},
                {"Полтора часа", TimeSpan.FromMinutes(90)},
                {"Два с половиной часа", TimeSpan.FromMinutes(150)}
            }
        );
        var schedule = container.Get<Schedule>();
        Assert.True(schedule.TryAddRecord(1, "1", DateTime.Today, "Полчаса"));
        var record = new ScheduleRecord(1, new TimeInterval(DateTime.Today, DateTime.Today.AddMinutes(30)), "1");
        Assert.AreEqual(schedule.GetRecordsTimesByUser(1)[0], record);
        Assert.IsEmpty(schedule.GetRecordsTimesByUser(2));
        Assert.True(schedule.TryRemoveRecord(record));
    }

    [Test]
    public void TestSchedule_WhenCannotAddRecord()
    {
        var container = Container;
        container.Bind<IRecordsRepository>()
            .To<MockScheduleRepository>()
            .WithConstructorArgument("freeTimes", new Dictionary<string, bool[]>
            {
                {"1", new bool[48 * 3]},
                {"2", new bool[48 * 3]},
                {"3", new bool[48 * 3]}
            })
            .WithConstructorArgument("dataBase", new Dictionary<long, List<ScheduleRecord>>());

        container.Bind<Schedule>().ToSelf().WithConstructorArgument("washingTypes",
            new Dictionary<string, TimeSpan>
            {
                {"1", TimeSpan.FromMinutes(30)},
                {"2", TimeSpan.FromMinutes(90)},
                {"3", TimeSpan.FromMinutes(150)}
            }
        );
        var schedule = container.Get<Schedule>();
        schedule.TryAddRecord(1, "1", DateTime.Today, "1");
        Assert.False(schedule.TryAddRecord(2, "1", DateTime.Today, "3"));
        Assert.False(schedule.TryAddRecord(2, "1", DateTime.Today.AddDays(-1), "3"));
        Assert.False(schedule.TryAddRecord(2, "1", DateTime.Today.AddDays(10000), "3"));
    }

    [Test]
    public void TestSchedule_WhenCannotRemoveRecord()
    {
        var container = Container;
        container.Bind<IRecordsRepository>()
            .To<MockScheduleRepository>()
            .WithConstructorArgument("freeTimes", new Dictionary<string, bool[]>
            {
                {"1", new bool[48 * 3]},
                {"2", new bool[48 * 3]},
                {"3", new bool[48 * 3]}
            })
            .WithConstructorArgument("dataBase", new Dictionary<long, List<ScheduleRecord>>());

        container.Bind<Schedule>().ToSelf().WithConstructorArgument("washingTypes",
            new Dictionary<string, TimeSpan>
            {
                {"1", TimeSpan.FromMinutes(30)},
                {"2", TimeSpan.FromMinutes(90)},
                {"3", TimeSpan.FromMinutes(150)}
            }
        );

        var schedule = container.Get<Schedule>();
        schedule.TryAddRecord(1, "1", DateTime.Today, "1");
        Assert.False(schedule.TryRemoveRecord(new ScheduleRecord(2,
            new TimeInterval(DateTime.Today, DateTime.Today.AddMinutes(30)), "1")));
        Assert.False(schedule.TryRemoveRecord(new ScheduleRecord(2,
            new TimeInterval(DateTime.Today, DateTime.Today.AddMinutes(150)), "1")));
    }

    [Test]
    public void TestSchedule_WhenNoFreeTime()
    {
        var container = Container;
        var ft = new bool[48 * 3];
        for (var i = 0; i < ft.Length; i++) ft[i] = true;
        container.Bind<IRecordsRepository>()
            .To<MockScheduleRepository>()
            .WithConstructorArgument("freeTimes", new Dictionary<string, bool[]>
            {
                {"1", ft.ToArray()},
                {"2", ft.ToArray()},
                {"3", ft.ToArray()}
            })
            .WithConstructorArgument("dataBase", new Dictionary<long, List<ScheduleRecord>>());

        container.Bind<Schedule>().ToSelf().WithConstructorArgument("washingTypes",
            new Dictionary<string, TimeSpan>
            {
                {"1", TimeSpan.FromMinutes(30)},
                {"2", TimeSpan.FromMinutes(90)},
                {"3", TimeSpan.FromMinutes(150)}
            }
        );

        var schedule = container.Get<Schedule>();
        Assert.False(schedule.TryAddRecord(1, "1", DateTime.Today, "3"));
        Assert.False(schedule.TryAddRecord(2, "2", DateTime.Today.AddHours(23), "2"));
        Assert.False(schedule.TryAddRecord(3, "3", DateTime.Today.AddDays(2), "3"));
        foreach (var freeTimes in schedule.GetFreeTimes().Values) Assert.IsEmpty(freeTimes);
    }
}