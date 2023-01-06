namespace DormitoryBot.Domain.Schedule
{
    public class Schedule
    {
        private readonly IRecordsRepository repository;
        private readonly Timer timer;
        private readonly Dictionary<string, TimeSpan> washingTypes = new Dictionary<string, TimeSpan>
                {
                    {"Полчаса", TimeSpan.FromMinutes(30)},
                    {"Полтора часа", TimeSpan.FromMinutes(90)},
                    {"Два с половиной часа", TimeSpan.FromMinutes(150)}
                };

        public Schedule(IRecordsRepository repository)
        {
            this.repository = repository;
            timer = new Timer(ClearPreviousDay, new object(), DateTime.Today.AddDays(1) - DateTime.Now,
                TimeSpan.FromDays(1));
        }

        public string[] MachineNames => 
            repository.FreeTimes.Keys.ToArray();

        public Dictionary<string, TimeSpan> WashingTypes =>
            washingTypes.ToDictionary(x => x.Key, pair => pair.Value);

        private void ClearPreviousDay(object stateInfo)
        {
            repository.ClearPreviousDay();
            Console.WriteLine("Cleared previous day");
        }

        public bool TryAddRecord(long user, string machine, DateTime startDate, string washingType)
        {
            var finishDate = startDate.Add(WashingTypes[washingType]);
            var record = new ScheduleRecord(user, new TimeInterval(startDate, finishDate), machine);
            if (record.TimeInterval.Start.Minute % 30 != 0)
                return false;
            var freeTimes = repository.FreeTimes[machine];
            var timeToCheck = startDate;
            while (timeToCheck < finishDate)
            {
                if (!freeTimes.Contains(timeToCheck)) return false;

                timeToCheck = timeToCheck.AddMinutes(30);
            }

            repository.AddRecord(record);
            return true;
        }

        public bool TryRemoveRecord(ScheduleRecord record)
        {
            var records = repository.GetRecordsByUser(record.User);
            if (!records.Contains(record)) return false;

            repository.RemoveRecord(record);
            return true;
        }

        public List<ScheduleRecord> GetRecordsTimesByUser(long user)
        {
            return repository.GetRecordsByUser(user);
        }

        // record(Guid Id, Guid UserId, nvarchar(max) MachineName, Date Start, Date End)

        public Dictionary<string, List<DateTime>> GetFreeTimes()
        {
            return repository.FreeTimes;
        }
    }
}