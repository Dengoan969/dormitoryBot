namespace DormitoryBot.Infrastructure
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
        
        DateTime Today { get; }
    }
}
