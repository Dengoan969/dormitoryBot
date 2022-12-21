namespace DomitoryBot.Domain;

public record ScheduleRecord
{
    public readonly string Machine;

    public readonly TimeInterval TimeInterval;

    //public readonly Guid guid;
    public readonly long User;

    public ScheduleRecord(long user, TimeInterval timeInterval, string machine)
    {
        //guid = Guid.NewGuid();
        User = user;
        TimeInterval = timeInterval;
        Machine = machine;
    }
}