using System.Configuration;
using DormitoryBot.Infrastructure;
using Npgsql;

namespace DormitoryBot.Domain.Schedule;

public class PgSqlScheduleRepository : IRecordsRepository
{
    private readonly Dictionary<string, bool[]> freeTimes;
    private readonly IDateTimeService dateTimeService;
    private static string connString = ConfigurationManager.ConnectionStrings["pgsql"].ConnectionString;

    public PgSqlScheduleRepository(Dictionary<string, bool[]> freeTimes,
        IDateTimeService dateTimeService)
    {
        this.freeTimes = freeTimes;
        this.dateTimeService = dateTimeService;
    }

    public PgSqlScheduleRepository() : this(
        new Dictionary<string, bool[]>
        {
            {"1", new bool[48 * 3]},
            {"2", new bool[48 * 3]},
            {"3", new bool[48 * 3]}
        },
        new DefaultDateTimeService()){}
    
    public void AddRecord(ScheduleRecord scheduleRecord)
    {
        var startIndex = GetIndexByDate(scheduleRecord.TimeInterval.Start);
        var endIndex = GetIndexByDate(scheduleRecord.TimeInterval.End);
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command = new NpgsqlCommand("INSERT INTO records (user_id, machine, start, finish)"+
                                              "VALUES (@1, @2, @3, @4)", conn);
        command.Parameters.AddWithValue("1", scheduleRecord.User);
        command.Parameters.AddWithValue("2", scheduleRecord.Machine);
        command.Parameters.AddWithValue("3", scheduleRecord.TimeInterval.Start);
        command.Parameters.AddWithValue("4", scheduleRecord.TimeInterval.End);
        command.ExecuteNonQuery();
        for (var i = startIndex; i < endIndex; i++)
        {
            freeTimes[scheduleRecord.Machine][i] = true;
        }
    }

    public void RemoveRecord(ScheduleRecord scheduleRecord)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command = new NpgsqlCommand("DELETE FROM records "+
                                              "WHERE user_id = @1 AND machine = @2 AND start = @3 AND finish = @4", conn);
        command.Parameters.AddWithValue("1", scheduleRecord.User);
        command.Parameters.AddWithValue("2", scheduleRecord.Machine);
        command.Parameters.AddWithValue("3", scheduleRecord.TimeInterval.Start);
        command.Parameters.AddWithValue("4", scheduleRecord.TimeInterval.End);
        command.ExecuteNonQuery();
        var startIndex = GetIndexByDate(scheduleRecord.TimeInterval.Start);
        var endIndex = GetIndexByDate(scheduleRecord.TimeInterval.End);
        for (var i = startIndex; i < endIndex; i++)
        {
            freeTimes[scheduleRecord.Machine][i] = false;
        }
    }

    public List<ScheduleRecord> GetRecordsByUser(long user)
    {
        var res = new List<ScheduleRecord>();
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command =
            new NpgsqlCommand("SELECT user_id, machine, start, finish "
                              +"FROM records WHERE user_id = @1", conn);
        command.Parameters.AddWithValue("1", user);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var userId = reader.GetFieldValue<long>(0);
            var machine = reader.GetFieldValue<string>(1);
            var start = reader.GetFieldValue<DateTime>(2).ToLocalTime();
            var end = reader.GetFieldValue<DateTime>(3).ToLocalTime();
            res.Add(new ScheduleRecord(userId, new TimeInterval(start, end), machine));
        }

        return res;
    }
    
    
    public Dictionary<string, List<DateTime>> FreeTimes
    {
        get
        {
            var today = dateTimeService.Today;
            var times = new Dictionary<string, List<DateTime>>();
            foreach (var machine in freeTimes.Keys)
            {
                times[machine] = new List<DateTime>();
                for (var i = 0; i < freeTimes[machine].Length; i++)
                    if (!freeTimes[machine][i])
                        times[machine].Add(today.AddMinutes(30 * i));
            }
            return times;
        }
    }

    public void ClearPreviousDay()
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command = new NpgsqlCommand("DELETE FROM records "+
                                              "WHERE start < @1", conn);
        command.Parameters.AddWithValue("1", dateTimeService.Today);
        command.ExecuteNonQuery();
    }
    private int GetIndexByDate(DateTime date)
    {
        var today = dateTimeService.Today;
        var diff = date - today;
        return diff.Days * 48 + diff.Hours * 2 + diff.Minutes / 30;
    }
}