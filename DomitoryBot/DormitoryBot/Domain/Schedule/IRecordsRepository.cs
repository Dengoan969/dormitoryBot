namespace DormitoryBot.Domain.Schedule
{
    public interface IRecordsRepository
    {
        void AddRecord(ScheduleRecord scheduleRecord);

        void RemoveRecord(ScheduleRecord scheduleRecord);

        List<ScheduleRecord> GetRecordsByUser(long user);

        Dictionary<string, List<DateTime>> FreeTimes { get; }

        void ClearPreviousDay();
    }
}