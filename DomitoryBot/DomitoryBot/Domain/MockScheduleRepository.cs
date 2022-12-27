namespace DomitoryBot.Domain;

public class MockScheduleRepository : IRecordsRepository
{
    private static readonly Dictionary<long, List<ScheduleRecord>> dataBase = new();

    private readonly Dictionary<string, bool[]> freeTimes = new()
    {
        {"1", new bool[48 * 3]},
        {"2", new bool[48 * 3]},
        {"3", new bool[48 * 3]}
    };

    public bool TryAddRecord(ScheduleRecord scheduleRecord)
    {
        if (scheduleRecord.TimeInterval.Start.Minute % 30 != 0 || scheduleRecord.TimeInterval.End.Minute % 30 != 0)
            return false;
        var startIndex = GetIndexByDate(scheduleRecord.TimeInterval.Start);
        var endIndex = GetIndexByDate(scheduleRecord.TimeInterval.End);
        for (var i = startIndex; i < endIndex; i++)
            if (freeTimes[scheduleRecord.Machine][i])
                return false;
        if (!dataBase.ContainsKey(scheduleRecord.User)) dataBase.Add(scheduleRecord.User, new List<ScheduleRecord>());
        dataBase[scheduleRecord.User].Add(scheduleRecord);
        for (var i = startIndex; i < endIndex; i++) freeTimes[scheduleRecord.Machine][i] = true;
        return true;
    }

    public bool TryRemoveRecord(ScheduleRecord scheduleRecord)
    {
        var startIndex = GetIndexByDate(scheduleRecord.TimeInterval.Start);
        var endIndex = GetIndexByDate(scheduleRecord.TimeInterval.End);
        for (var i = startIndex; i < endIndex; i++)
            if (!freeTimes[scheduleRecord.Machine][i])
                return false;
        dataBase[scheduleRecord.User] = dataBase[scheduleRecord.User]
            .Where(x => x != scheduleRecord).ToList();

        for (var i = startIndex; i < endIndex; i++) freeTimes[scheduleRecord.Machine][i] = false;
        return true;
    }


    public List<ScheduleRecord> GetRecordsTimesByUser(long user)
    {
        if (dataBase.TryGetValue(user, out var records))
            return records;

        return new List<ScheduleRecord>();
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

    public void ClearPreviousDay()
    {
        foreach (var machine in freeTimes.Keys)
        {
            var newDay = new bool[48 * 3];
            for (var i = 0; i < 96; i++) newDay[i] = freeTimes[machine][i + 48];
            freeTimes[machine] = newDay;
        }

        foreach (var user in dataBase.Keys)
            dataBase[user] = dataBase[user].Where(x => x.TimeInterval.Start >= DateTime.Today).ToList();
    }

    private int GetIndexByDate(DateTime date)
    {
        var today = DateTime.Today;
        var diff = date - today;
        return diff.Days * 48 + diff.Hours * 2 + diff.Minutes / 30;
    }
}