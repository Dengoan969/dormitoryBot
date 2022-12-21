namespace DomitoryBot.Domain;

public interface IRecordsRepository
{
    // todo to bool
    void AddRecord(ScheduleRecord scheduleRecord);
    void RemoveRecord(ScheduleRecord scheduleRecord);
    List<ScheduleRecord> GetRecordsTimesByUser(long user);
    Dictionary<string, List<DateTime>> GetFreeTimes();
}