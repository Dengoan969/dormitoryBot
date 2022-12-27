using DormitoryBot.Domain;
using DormitoryBot.Infrastructure;

namespace DormitoryBot.App
{
    public class Schedule
    {
        public readonly string[] machineNames;
        private readonly Timer timer;
        public readonly Dictionary<string, TimeSpan> washingTypes;
        private IRecordsRepository data;

        public Schedule(IRecordsRepository data, Dictionary<string, TimeSpan> washingTypes)
        {
            this.data = data;
            this.washingTypes = washingTypes;
            machineNames = data.GetFreeTimes().Keys.ToArray();
            timer = new Timer(ClearPreviousDay, new object(), DateTime.Today.AddDays(1) - DateTime.Now,
                TimeSpan.FromDays(1));
        }

        private void ClearPreviousDay(object stateInfo)
        {
            data.ClearPreviousDay();
            Console.WriteLine("Cleared previous day");
        }

        public bool TryAddRecord(long user, string machine, DateTime startDate, string washingType)
        {
            var finishDate = startDate.Add(washingTypes[washingType]);
            var record = new ScheduleRecord(user, new TimeInterval(startDate, finishDate), machine);
            if (record.TimeInterval.Start.Minute % 30 != 0)
                return false;
            var freeTimes = data.GetFreeTimes()[machine];
            var timeToCheck = startDate;
            while (timeToCheck < finishDate)
            {
                if (!freeTimes.Contains(timeToCheck)) return false;

                timeToCheck = timeToCheck.AddMinutes(30);
            }

            data.AddRecord(record);
            return true;
        }

        public bool TryRemoveRecord(ScheduleRecord record)
        {
            var records = data.GetRecordsTimesByUser(record.User);
            if (!records.Contains(record)) return false;

            data.RemoveRecord(record);
            return true;
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