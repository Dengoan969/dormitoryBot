using DormitoryBot.Domain;

namespace DormitoryBot.Infrastructure;

public interface IRecordsRepository
{
    void AddRecord(ScheduleRecord scheduleRecord);
    void RemoveRecord(ScheduleRecord scheduleRecord);
    List<ScheduleRecord> GetRecordsTimesByUser(long user);
    Dictionary<string, List<DateTime>> GetFreeTimes();
    void ClearPreviousDay();
}