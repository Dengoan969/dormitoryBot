using DomitoryBot.Domain;

namespace DomitoryBot.Commands;

public interface IRecordsRepository
{
    bool TryAddRecord(ScheduleRecord scheduleRecord);
    bool TryRemoveRecord(ScheduleRecord scheduleRecord);
    List<ScheduleRecord> GetRecordsTimesByUser(long user);
    Dictionary<string, List<DateTime>> GetFreeTimes();
}