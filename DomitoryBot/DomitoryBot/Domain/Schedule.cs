using System.Diagnostics.CodeAnalysis;

namespace DomitoryBot.Domain
{

    public readonly struct TimeInterval //TimeSpan?????
    {
        public readonly DateTime Start;
        public readonly DateTime End;

        public TimeInterval(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(DateTime moment)
        {
            return moment >= Start && moment <= End;
        }

        public override bool Equals(object? obj) => obj is TimeInterval other && other.Start == Start && other.End == End;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(TimeInterval x, TimeInterval y) => x.Equals(y);
        public static bool operator !=(TimeInterval x, TimeInterval y) => !(x==y);
    }

    public record Record
    {
        //public readonly Guid guid;
        public readonly Guid User;
        public readonly string Machine;
        public readonly TimeInterval TimeInterval;

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
            { WashingType.fast, TimeSpan.FromMinutes(30) },
            { WashingType.b, TimeSpan.FromMinutes(60) },
            { WashingType.c, TimeSpan.FromMinutes(90) }, //ADD WASHING TYPES
        };
        private IRecordsRepository data;
        private readonly string[] machineNames;

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
                    if (timeInterval.Contains(startDate) && timeInterval.Contains(finishDate))
                    {
                        var washingInterval = new TimeInterval(startDate, finishDate);
                        var record = new Record(user, timeInterval, machine);
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

        public Dictionary<string, TimeInterval[]> GetFreeTimes() => data.GetFreeTimes();
    }

    public interface IRecordsRepository
    {
        void AddRecord(Record record);
        void RemoveRecord(Record record);
        Dictionary<string, TimeInterval[]> GetRecordsTimes(Guid user);
        Dictionary<string, TimeInterval[]> GetFreeTimes();
    }

    public class MockRepo : IRecordsRepository
    {
        private static readonly Dictionary<Guid, List<Record>> dataBase = new Dictionary<Guid, List<Record>>();

        public void AddRecord(Record record)
        {
            if (!dataBase.ContainsKey(record.User))
            {
                dataBase.Add(record.User, new List<Record>());
            }
            dataBase[record.User].Add(record);
        }

        public void RemoveRecord(Record record)
        {
            dataBase[record.User] = dataBase[record.User]
                .Where(x => x != record).ToList();
        }

        public Dictionary<string, TimeInterval[]> GetRecordsTimes(Guid user)
        {
            //data.GetRecords().Where(x => x.guid == user).GroupBy...
            throw new NotImplementedException();
        }

        public Dictionary<string, TimeInterval[]> GetFreeTimes()
        {
            // 3 5
            //hashset free 2[]8
            //0[]2 6[]9
            //data.
            throw new NotImplementedException();
        }
    }
}