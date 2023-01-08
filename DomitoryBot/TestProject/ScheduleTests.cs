using System;
using System.Collections.Generic;
using DormitoryBot.Domain.Schedule;
using DormitoryBot.Domain;
using NUnit.Framework;
using FakeItEasy;

namespace TestProject
{
    public class ScheduleTests
    {
        private IRecordsRepository repository;
        private Schedule schedule;

        [SetUp]
        public void Setup()
        {
            repository = A.Fake<IRecordsRepository>();
            schedule = new Schedule(repository);
        }

        [Test]
        public void TestSchedule_TryAddRecord_WhenWrongTimeFormat()
        {
            var washingType = "Полчаса";
            A.CallTo(() => repository.FreeTimes).Returns(A.Dummy<Dictionary<string, List<DateTime>>>());
            Assert.False(schedule.TryAddRecord(A.Dummy<long>(), A.Dummy<string>(),
                DateTime.Today.AddMinutes(42), washingType));
            Assert.False(schedule.TryAddRecord(A.Dummy<long>(), A.Dummy<string>(),
                DateTime.Today.AddMinutes(10), washingType));
        }

        [Test]
        public void TestSchedule_TryAddRecord_WhenNoFreeTime()
        {
            var machineName = A.Dummy<string>();
            var washingType = "Полчаса";
            A.CallTo(() => repository.FreeTimes).Returns(new Dictionary<string, List<DateTime>>
            {
                {machineName, new List<DateTime>()},
            });
            Assert.False(schedule.TryAddRecord(A.Dummy<long>(), machineName, A.Dummy<DateTime>(), washingType));
        }

        [Test]
        public void TestSchedule_TryAddRecord_WhenNoFitFreeTime()
        {
            var machineName = A.Dummy<string>();
            var washingType = "Полчаса";
            A.CallTo(() => repository.FreeTimes).Returns(new Dictionary<string, List<DateTime>>
            {
                {machineName, new List<DateTime>(){DateTime.Today, DateTime.Today.AddMinutes(50)} },
            });
            Assert.False(schedule.TryAddRecord(A.Dummy<long>(), machineName, DateTime.Today.AddMinutes(42), washingType));
        }

        [Test]
        public void TestSchedule_TryAddRecord_CallAddRecord()
        {
            var user = A.Dummy<long>();
            var machineName = A.Dummy<string>();
            var washingType = "Полчаса";
            var date = A.Dummy<DateTime>();
            A.CallTo(() => repository.FreeTimes).Returns(new Dictionary<string, List<DateTime>>
            {
                {machineName, new List<DateTime>(){date} },
            });
            Assert.True(schedule.TryAddRecord(user, machineName, date, washingType));
            var finishDate = date.Add(TimeSpan.FromMinutes(30));
            var record = new ScheduleRecord(user, new TimeInterval(date, finishDate), machineName);
            A.CallTo(() => repository.AddRecord(record)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void TestSchedule_TryRemoveRecord_WhenNoRecords()
        {
            var record = A.Dummy<ScheduleRecord>();
            A.CallTo(() => repository.GetRecordsByUser(record.User)).Returns(new List<ScheduleRecord>());
            Assert.False(schedule.TryRemoveRecord(record));
        }

        public void TestSchedule_TryRemoveRecord_WhenFitRecord()
        {
            var record = A.Dummy<ScheduleRecord>();
            A.CallTo(() => repository.GetRecordsByUser(record.User)).Returns(new List<ScheduleRecord>() { });
            Assert.False(schedule.TryRemoveRecord(record));
        }

        [Test]
        public void TestSchedule_TryRemoveRecord_CallGetUserRecords_CallRemoveRecord()
        {
            var record = A.Dummy<ScheduleRecord>();
            A.CallTo(() => repository.GetRecordsByUser(record.User)).Returns(new List<ScheduleRecord>() { record });
            Assert.True(schedule.TryRemoveRecord(record));
            A.CallTo(() => repository.GetRecordsByUser(record.User)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.RemoveRecord(record)).MustHaveHappenedOnceExactly();
        }
    }
}