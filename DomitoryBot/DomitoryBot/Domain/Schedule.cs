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
        public readonly Guid User;

        public Record(Guid user, TimeInterval timeInterval, string machine)
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
            {WashingType.c, TimeSpan.FromMinutes(90)}, //ADD WASHING TYPES
        };

        private readonly string[] machineNames;
        private IRecordsRepository data;

        public Schedule(IRecordsRepository data, string[] machineNames)
        {
            this.data = data;
            this.machineNames = machineNames;
        }

        public bool AddRecord(Guid user, string machine, DateTime startDate, WashingType washingType)
        {
            var finishDate = startDate.Add(washingTypes[washingType]);
            foreach (var pair in data.GetFreeTimes())
            {
                foreach (var timeInterval in pair.Value)
                {
                    var washingInterval = new TimeInterval(startDate, finishDate);
                    if (timeInterval.Contains(washingInterval))
                    {
                        var record = new Record(user, washingInterval, machine);
                        data.AddRecord(record);
                        return true;
                    }
                }
            }

            return false;
        }

        public void RemoveRecord(Guid user, TimeInterval timeInterval, string machine)
        {
            var record = new Record(user, timeInterval, machine);
            data.RemoveRecord(record);
        }


        // record(Guid Id, Guid UserId, nvarchar(max) MachineName, Date Start, Date End)

        public Dictionary<string, List<TimeInterval>> GetFreeTimes()
        {
            return data.GetFreeTimes();
        }
    }

    public interface IRecordsRepository
    {
        void AddRecord(Record record);
        void RemoveRecord(Record record);
        Dictionary<string, List<TimeInterval>> GetRecordsTimesByUser(Guid user);
        Dictionary<string, List<TimeInterval>> GetFreeTimes();
    }

    public class ScheduleMockRepository : IRecordsRepository
    {
        private static readonly Dictionary<Guid, List<Record>> dataBase = new Dictionary<Guid, List<Record>>();

        private readonly Dictionary<string, List<TimeInterval>>
            freeTimes; //todo: list vs hashset. How to work work intervals

        public ScheduleMockRepository(Dictionary<string, List<TimeInterval>> freeTimes)
        {
            this.freeTimes = freeTimes;
        }

        public void AddRecord(Record record)
        {
            if (freeTimes[record.Machine].All(x => !x.Contains(record.TimeInterval))) throw new ArgumentException("");
            if (!dataBase.ContainsKey(record.User))
            {
                dataBase.Add(record.User, new List<Record>());
            }

            dataBase[record.User].Add(record);
            var timeInterval = record.TimeInterval;
            var indexToSeparate = freeTimes[record.Machine]
                .FindIndex(x => x.Contains(timeInterval.Start));
            var toAdd = freeTimes[record.Machine][indexToSeparate].ExcludeInterval(timeInterval);
            freeTimes[record.Machine].RemoveAt(indexToSeparate);
            for (var i = indexToSeparate; i < indexToSeparate + toAdd.Length; i++)
                freeTimes[record.Machine].Insert(i, toAdd[i - indexToSeparate]);
        }

        public void RemoveRecord(Record record)
        {
            if (freeTimes[record.Machine].Any(x => x.Contains(record.TimeInterval))) throw new ArgumentException("");
            dataBase[record.User] = dataBase[record.User]
                .Where(x => x != record).ToList();
            var timeInterval = record.TimeInterval;
            var indexToInsert = freeTimes[record.Machine].FindIndex(x => x.Start >= record.TimeInterval.End);
            if (indexToInsert == -1)
                indexToInsert = freeTimes.Count;
            //todo
        }


        public Dictionary<string, List<TimeInterval>> GetRecordsTimesByUser(Guid user)
        {
            if (dataBase.TryGetValue(user, out var records))
                return records.GroupBy(x => x.Machine)
                    .ToDictionary(grouping => grouping.Key,
                        grouping => grouping.Select(x => x.TimeInterval).ToList());

            return new Dictionary<string, List<TimeInterval>>();
        }

        public Dictionary<string, List<TimeInterval>> GetFreeTimes()
        {
            // 3 5
            //hashset free 2[]8
            //0[]2 6[]9
            //data.
            return freeTimes;
        }
    }
}