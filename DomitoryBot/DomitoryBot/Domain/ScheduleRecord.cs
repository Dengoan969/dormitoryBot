namespace DomitoryBot.Domain;

public record ScheduleRecord(long User, TimeInterval TimeInterval, string Machine)
{
    //guid = Guid.NewGuid();
    public readonly string Machine = Machine;

    public readonly TimeInterval TimeInterval = TimeInterval;

    //public readonly Guid guid;
    public readonly long User = User;
}