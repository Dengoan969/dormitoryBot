namespace DomitoryBot.Domain
{
    public class Schedule
    {
        public static readonly Dictionary<WashingType, TimeSpan> washingTypes = new()
        {
            {WashingType.fast, TimeSpan.FromMinutes(30)},
            {WashingType.medium, TimeSpan.FromMinutes(60)},
            {WashingType.slow, TimeSpan.FromMinutes(90)} //todo ADD WASHING TYPES
        };

        public readonly string[] machineNames = {"1", "2", "3"};
        private IRecordsRepository data;

        public Schedule(IRecordsRepository data)
        {
            this.data = data;
        }

        public bool AddRecord(long user, string machine, DateTime startDate, WashingType washingType)
        {
            var finishDate = startDate.Add(washingTypes[washingType]);
            var record = new ScheduleRecord(user, new TimeInterval(startDate, finishDate), machine);
            try
            {
                data.AddRecord(record);
            }
            catch (ArgumentException e)
            {
                return false;
            }

            return true;
        }

        public bool RemoveRecord(long user, TimeInterval timeInterval, string machine)
        {
            var record = new ScheduleRecord(user, timeInterval, machine);
            try
            {
                data.RemoveRecord(record);
            }
            catch (ArgumentException e)
            {
                return false;
            }

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