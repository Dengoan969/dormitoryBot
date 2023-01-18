using System;

namespace DormitoryBot.Infrastructure
{
    public class DefaultDateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;

        public DateTime Today => DateTime.Today;
    }
}
