﻿using DomitoryBot.Domain;
using DomitoryBot.Infrastructure;

namespace DomitoryBot.App
{
    public class Schedule
    {
        public static readonly Dictionary<WashingType, TimeSpan> washingTypes = new()
        {
            {WashingType.fast, TimeSpan.FromMinutes(30)},
            {WashingType.medium, TimeSpan.FromMinutes(60)},
            {WashingType.slow, TimeSpan.FromMinutes(90)} //todo ADD WASHING TYPES
        };

        public readonly string[] machineNames;
        private readonly Timer timer;
        private IRecordsRepository data;

        public Schedule(IRecordsRepository data)
        {
            this.data = data;
            machineNames = data.GetFreeTimes().Keys.ToArray();
            timer = new Timer(ClearPreviousDay, new object(), DateTime.Today.AddDays(1) - DateTime.Now,
                TimeSpan.FromDays(1));
        }

        private void ClearPreviousDay(object stateInfo)
        {
            data.ClearPreviousDay();
            Console.WriteLine("Cleared previous day");
        }

        public bool AddRecord(long user, string machine, DateTime startDate, WashingType washingType)
        {
            var finishDate = startDate.Add(washingTypes[washingType]);
            var record = new ScheduleRecord(user, new TimeInterval(startDate, finishDate), machine);
            return data.TryAddRecord(record);
        }

        public bool RemoveRecord(ScheduleRecord record)
        {
            return data.TryRemoveRecord(record);
        }

        public List<ScheduleRecord> GetRecordsTimesByUser(long user)
        {
            return data.GetRecordsTimesByUser(user);
        }

        // record(Guid Id, Guid UserId, nvarchar(max) MachineName, Date Start, Date End)

        public Dictionary<string, List<DateTime>> GetFreeTimes()
        {
            return data.GetFreeTimes();
        }
    }
}