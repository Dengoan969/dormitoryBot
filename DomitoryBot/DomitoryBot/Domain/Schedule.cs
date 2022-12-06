namespace DomitoryBot.Domain
{
    public enum WashingType
    {
        a,
        b,
        c
    }

    public class TimeInterval
    {
        public readonly DateTime End;
        public readonly DateTime Start;

        public TimeInterval(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(DateTime moment)
        {
            return moment >= Start && moment <= End;
        }

        public override bool Equals(object? obj)
        {
            return Equals((TimeInterval) obj);
        }

        private bool Equals(TimeInterval? timeInterval)
        {
            return timeInterval != null && timeInterval.Start == Start && timeInterval.End == End;
        }
    }

    public class Schedule
    {
        private IRecordsRepository data;
        string[] machineNames;

        public Schedule(IRecordsRepository data)
        {
            this.data = data;
        }

        public bool AddRecord(Guid User, string machine, DateTime date, WashingType washingType)
        {
            foreach (var pair in GetFreeTimes())
            {
                foreach (var timeInterval in pair.Value)
                {
                    if (timeInterval.Contains(date.AddSeconds((double) washingType)))
                    {
                    }
                }
            }

            throw new NotImplementedException();
        }

        public void RemoveRecord(Guid user, TimeInterval timeInterval, string machine)
        {
            throw new NotImplementedException();
        }

        public DateTime GetRecordTime(Guid user)
        {
            //data.GetRecord(user).Time;
            throw new NotImplementedException();
        }

        public Dictionary<string, TimeInterval[]> GetFreeTimes()
        {
            throw new NotImplementedException();
        }
    }

    public interface IRecordsRepository // Взять у Рината
    {
        void AddRecord(Guid User, string machine, TimeInterval timeInterval);
        void RemoveRecord(Guid User, TimeInterval timeInterval, string machine);
        void GetRecords();
        void GetRecord();
    }

    public interface ITimeTable // Взять у Рината
    {
    }

    public class MockRepo : IRecordsRepository
    {
        private readonly Dictionary<Guid, List<Record>> dataBase;

        public MockRepo()
        {
            dataBase = new Dictionary<Guid, List<Record>>();
        }


        public void AddRecord(Guid User, string machine, TimeInterval timeInterval)
        {
            if (!dataBase.ContainsKey(User)) dataBase.Add(User, new List<Record>());
            dataBase[User].Add(new Record(User, timeInterval, machine));
        }

        public void RemoveRecord(Guid User, TimeInterval timeInterval, string machine)
        {
            dataBase[User] = dataBase[User]
                .Where(x => x.machine == machine && x.TimeInterval == timeInterval).ToList();
        }

        public void GetRecords()
        {
            throw new NotImplementedException();
        }

        public void GetRecord()
        {
            throw new NotImplementedException();
        }
    }

    public class Record
    {
        public readonly Guid Guid;
        public readonly string machine;
        public readonly TimeInterval TimeInterval;

        public Record(Guid guid, TimeInterval timeInterval, string machine)
        {
            Guid = guid;
            TimeInterval = timeInterval;
            this.machine = machine;
        }
    }
}