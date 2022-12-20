namespace DomitoryBot.Domain
{
    public readonly struct TimeInterval //TimeSpan?????
    {
        public readonly DateTime Start;
        public readonly DateTime End;

        public TimeInterval(DateTime start, DateTime end)
        {
            if (start >= end) throw new ArgumentException();
            Start = start;
            End = end;
        }

        public bool Contains(DateTime moment)
        {
            return moment >= Start && moment <= End;
        }

        public bool Contains(TimeInterval other)
        {
            return other.Start >= Start && other.End <= End;
        }

        public override bool Equals(object? obj) =>
            obj is TimeInterval other && other.Start == Start && other.End == End;

        public override int GetHashCode()
        {
            unchecked
            {
                return 301577 * Start.GetHashCode() + 625663 * End.GetHashCode();
            }
        }

        public TimeInterval[] ExcludeInterval(TimeInterval other)
        {
            if (!Contains(other)) throw new InvalidOperationException();

            var ans = new List<TimeInterval>();

            if (Start != other.Start) ans.Add(new TimeInterval(Start, other.Start));

            if (End != other.End) ans.Add(new TimeInterval(other.End, End));

            return ans.ToArray();
        }

        public static bool operator ==(TimeInterval x, TimeInterval y) => x.Equals(y);
        public static bool operator !=(TimeInterval x, TimeInterval y) => !(x == y);
    }

    public record Record
    {
        public readonly string Machine;

        public readonly TimeInterval TimeInterval;

        //public readonly Guid guid;
        public readonly long User;

        public Record(long user, TimeInterval timeInterval, string machine)
        {
            //guid = Guid.NewGuid();
            User = user;
            TimeInterval = timeInterval;
            Machine = machine;
        }
    }

    public enum WashingType
    {
        fast,
        b,
        c
    }

    public class Schedule
    {
        private static readonly Dictionary<WashingType, TimeSpan> washingTypes = new Dictionary<WashingType, TimeSpan>
        {
            {WashingType.fast, TimeSpan.FromMinutes(30)},
            {WashingType.b, TimeSpan.FromMinutes(60)},
            {WashingType.c, TimeSpan.FromMinutes(90)} //todo ADD WASHING TYPES
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
            var record = new Record(user, new TimeInterval(startDate, finishDate), machine);
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
            var record = new Record(user, timeInterval, machine);
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

        public List<Record> GetRecordsTimesByUser(long user)
        {
            return data.GetRecordsTimesByUser(user);
        }

        // record(Guid Id, Guid UserId, nvarchar(max) MachineName, Date Start, Date End)

        public Dictionary<string, List<DateTime>> GetFreeTimes()
        {
            return data.GetFreeTimes();
        }
    }

    public interface IRecordsRepository
    {
        void AddRecord(Record record);
        void RemoveRecord(Record record);
        List<Record> GetRecordsTimesByUser(long user);
        Dictionary<string, List<DateTime>> GetFreeTimes();
    }

    public class MockScheduleRepository : IRecordsRepository
    {
        private static readonly Dictionary<long, List<Record>> dataBase = new();

        private readonly Dictionary<string, bool[]> freeTimes = new()
        {
            {"1", new bool[48 * 3]},
            {"2", new bool[48 * 3]},
            {"3", new bool[48 * 3]}
        };

        public void AddRecord(Record record)
        {
            var startIndex = GetIndexByDate(record.TimeInterval.Start);
            var endIndex = GetIndexByDate(record.TimeInterval.End);
            for (var i = startIndex; i < endIndex; i++)
                if (freeTimes[record.Machine][i])
                    throw new ArgumentException();
            if (!dataBase.ContainsKey(record.User))
            {
                dataBase.Add(record.User, new List<Record>());
            }

            for (var i = startIndex; i < endIndex; i++) freeTimes[record.Machine][i] = true;
        }

        public void RemoveRecord(Record record)
        {
            var startIndex = GetIndexByDate(record.TimeInterval.Start);
            var endIndex = GetIndexByDate(record.TimeInterval.End);
            for (var i = startIndex; i < endIndex; i++)
                if (!freeTimes[record.Machine][i])
                    throw new ArgumentException();
            dataBase[record.User] = dataBase[record.User]
                .Where(x => x != record).ToList();

            for (var i = startIndex; i < endIndex; i++) freeTimes[record.Machine][i] = false;
        }


        public List<Record> GetRecordsTimesByUser(long user)
        {
            if (dataBase.TryGetValue(user, out var records))
                return records;

            return new List<Record>();
        }

        public Dictionary<string, List<DateTime>> GetFreeTimes()
        {
            var today = DateTime.Today;
            var times = new Dictionary<string, List<DateTime>>();
            foreach (var machine in freeTimes.Keys)
            {
                times[machine] = new List<DateTime>();
                var begin = today;
                for (var i = 0; i < freeTimes[machine].Length; i++)
                    if (!freeTimes[machine][i])
                        times[machine].Add(today.AddMinutes(30 * i));
            }

            return times;
        }

        private int GetIndexByDate(DateTime date)
        {
            var today = DateTime.Today;
            var diff = date - today;
            return diff.Days * 48 + diff.Hours * 2 + diff.Minutes / 30;
        }
    }
}