namespace DomitoryBot.Domain;

public readonly struct TimeInterval //TimeSpan?????
{
    public readonly DateTime Start;
    public readonly DateTime End;

    public TimeInterval(DateTime start, DateTime end)
    {
        if (start >= end) throw new ArgumentException();
        Start = start;
        End = end;
    }

    public bool Contains(DateTime moment)
    {
        return moment >= Start && moment <= End;
    }

    public bool Contains(TimeInterval other)
    {
        return other.Start >= Start && other.End <= End;
    }

    public override bool Equals(object? obj)
    {
        return obj is TimeInterval other && other.Start == Start && other.End == End;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return 301577 * Start.GetHashCode() + 625663 * End.GetHashCode();
        }
    }

    public TimeInterval[] ExcludeInterval(TimeInterval other)
    {
        if (!Contains(other)) throw new InvalidOperationException();

        var ans = new List<TimeInterval>();

        if (Start != other.Start) ans.Add(new TimeInterval(Start, other.Start));

        if (End != other.End) ans.Add(new TimeInterval(other.End, End));

        return ans.ToArray();
    }

    public static bool operator ==(TimeInterval x, TimeInterval y)
    {
        return x.Equals(y);
    }

    public static bool operator !=(TimeInterval x, TimeInterval y)
    {
        return !(x == y);
    }
}