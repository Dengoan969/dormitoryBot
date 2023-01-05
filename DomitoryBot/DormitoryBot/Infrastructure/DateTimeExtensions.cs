using DormitoryBot.Domain;

namespace DormitoryBot.Infrastructure;

public static class DateTimeExtensions
{
    public static List<TimeInterval> UnionDateTimeIntervals(this List<DateTime> times, int interval)
    {
        var begin = DateTime.MinValue;
        var last = DateTime.MinValue;
        var res = new List<TimeInterval>();
        foreach (var date in times)
        {
            if (begin == DateTime.MinValue && last == DateTime.MinValue)
            {
                begin = date;
                last = date;
                continue;
            }

            if (date - last == TimeSpan.FromMinutes(interval))
            {
                last = date;
            }

            else if (last != begin)
            {
                res.Add(new TimeInterval(begin, last.AddMinutes(interval)));
                begin = date;
                last = date;
            }

            else
            {
                begin = date;
                last = date;
            }
        }

        if (!(begin == DateTime.MinValue && last == DateTime.MinValue) && begin != last)
            res.Add(new TimeInterval(begin, last.AddMinutes(interval)));
        return res;
    }
}