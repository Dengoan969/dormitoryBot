﻿using DormitoryBot.Domain.Schedule;

namespace DomitoryBot.Domain.Schedule;

public interface IRecordsRepository
{
    void AddRecord(ScheduleRecord scheduleRecord);
    void RemoveRecord(ScheduleRecord scheduleRecord);
    List<ScheduleRecord> GetRecordsTimesByUser(long user);
    Dictionary<string, List<DateTime>> GetFreeTimes();
    void ClearPreviousDay();
}