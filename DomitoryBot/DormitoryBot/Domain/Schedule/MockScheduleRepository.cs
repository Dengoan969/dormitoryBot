using DormitoryBot.Infrastructure;

namespace DormitoryBot.Domain.Schedule
{
    public class MockScheduleRepository : IRecordsRepository
    {
        private readonly Dictionary<long, List<ScheduleRecord>> dataBase;

        private readonly Dictionary<string, bool[]> freeTimes;

        private readonly IDateTimeService dateTimeService;

        public MockScheduleRepository(Dictionary<string, bool[]> freeTimes,
                                      Dictionary<long, List<ScheduleRecord>> dataBase,
                                      IDateTimeService dateTimeService)
        {
            this.freeTimes = freeTimes;
            this.dataBase = dataBase;
            this.dateTimeService = dateTimeService;
        }

        public MockScheduleRepository() : this(
            new Dictionary<string, bool[]>
                    {
                    {"1", new bool[48 * 3]},
                    {"2", new bool[48 * 3]},
                    {"3", new bool[48 * 3]}
                    },
            new Dictionary<long, List<ScheduleRecord>>(),
            new DefaultDateTimeService())
        { }

        public void AddRecord(ScheduleRecord record)
        {
            var startIndex = GetIndexByDate(record.TimeInterval.Start);
            var endIndex = GetIndexByDate(record.TimeInterval.End);
            if (!dataBase.ContainsKey(record.User))
            {
                dataBase.Add(record.User, new List<ScheduleRecord>());
            }
            dataBase[record.User].Add(record);
            for (var i = startIndex; i < endIndex; i++)
            {
                freeTimes[record.Machine][i] = true;
            }
        }

        public void RemoveRecord(ScheduleRecord scheduleRecord)
        {
            var startIndex = GetIndexByDate(scheduleRecord.TimeInterval.Start);
            var endIndex = GetIndexByDate(scheduleRecord.TimeInterval.End);
            dataBase[scheduleRecord.User].Remove(scheduleRecord);
            for (var i = startIndex; i < endIndex; i++)
            {
                freeTimes[scheduleRecord.Machine][i] = false;
            }
        }

        public List<ScheduleRecord> GetRecordsByUser(long user)
        {
            return dataBase.TryGetValue(user, out var records) 
                ? records
                : new List<ScheduleRecord>();
        }

        public Dictionary<string, List<DateTime>> FreeTimes
        {
            get
            {
                var today = dateTimeService.Today;
                var times = new Dictionary<string, List<DateTime>>();
                foreach (var machine in freeTimes.Keys)
                {
                    times[machine] = new List<DateTime>();
                    for (var i = 0; i < freeTimes[machine].Length; i++)
                        if (!freeTimes[machine][i])
                            times[machine].Add(today.AddMinutes(30 * i));
                }
                return times;
            }
        }

        public void ClearPreviousDay()
        {
            foreach (var machine in freeTimes.Keys)
            {
                var newDay = new bool[48 * 3];
                for (var i = 0; i < 96; i++) newDay[i] = freeTimes[machine][i + 48];
                freeTimes[machine] = newDay;
            }
            foreach (var user in dataBase.Keys)
                dataBase[user] = dataBase[user].Where(x => x.TimeInterval.Start >= dateTimeService.Today).ToList();
        }

        private int GetIndexByDate(DateTime date)
        {
            var today = dateTimeService.Today;
            var diff = date - today;
            return diff.Days * 48 + diff.Hours * 2 + diff.Minutes / 30;
        }
    }
}