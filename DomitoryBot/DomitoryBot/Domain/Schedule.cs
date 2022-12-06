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
        private readonly DateTime start;
        private readonly DateTime end;
        public TimeInterval(DateTime start, DateTime end)
        {
            this.start = start;
            this.end = end;
        }

        public bool Contains(DateTime moment)
        {
            return moment >= start && moment <= end;
        }
    }

    public class Schedule
    {
        IScheduleData data;
        string[] machineNames;

        public Schedule(IScheduleData data) => this.data = data;

        public bool AddRecord(Guid User, string machine, DateTime date, WashingType washingType)
        {
            foreach(var pair in GetFreeTimes())
            {
                foreach(var timeInterval in pair.Value)
                {
                    if(timeInterval.Contains(date.AddSeconds((double)washingType)))
                    {

                    }
                }
            }
            throw new NotImplementedException();
        }

        public void RemoveRecord(Guid user)
        {
            data.RemoveRecord(user);
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



    public interface IScheduleData // Взять у Рината
    {
        bool AddRecord();
        void RemoveRecord(Guid id);
        void GetRecords();
        void GetRecord();
    }

    public interface ITimeTable // Взять у Рината
    {
    }
}
