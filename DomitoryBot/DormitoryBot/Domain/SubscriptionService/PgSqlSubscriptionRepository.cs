

using System.Configuration;
using Npgsql;

namespace DormitoryBot.Domain.SubscriptionService;

public class PgSqlSubscriptionRepository : ISubscriptionRepository
{
    private static string connString = ConfigurationManager.ConnectionStrings["pgsql"].ConnectionString;
    public string[] AllSubscriptions
    {
        get
        {
            var res = new List<string>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var command = new NpgsqlCommand("SELECT name FROM subs", conn);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                res.Add(reader.GetFieldValue<string>(0));
            }

            return res.ToArray();
        }
    }

    public string[] GetSubscriptionsOfUser(long userId)
    {
        var res = new List<string>();
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command =
            new NpgsqlCommand(
                "SELECT s.name FROM subs_rights sr JOIN subs s on s.id = sr.sub_id WHERE user_id = @1", conn);
        command.Parameters.AddWithValue("1", userId);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            res.Add(reader.GetFieldValue<string>(0));
        }

        return res.ToArray();
    }

    public string[] GetAdminSubscriptionsOfUser(long userId)
    {
        var res = new List<string>();
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command =
            new NpgsqlCommand(
                "SELECT s.name FROM subs_rights sr JOIN subs s on s.id = sr.sub_id "
                +"WHERE sr.user_id = @1 AND sr.privilege = 'Admin'", conn);
        command.Parameters.AddWithValue("1", userId);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            res.Add(reader.GetFieldValue<string>(0));
        }

        return res.ToArray();
    }

    public long[] GetFollowers(string sub)
    {
        var res = new List<long>();
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command =
            new NpgsqlCommand(
                "SELECT sr.user_id FROM subs_rights sr JOIN subs s ON s.id = sr.sub_id WHERE s.name = @1", conn);
        command.Parameters.AddWithValue("1", sub);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            res.Add(reader.GetFieldValue<long>(0));
        }

        return res.ToArray();
    }

    public long[] GetAdmins(string sub)
    {
        var res = new List<long>();
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command =
            new NpgsqlCommand(
                "SELECT sr.user_id FROM subs_rights sr "+
                "JOIN subs s ON s.id = sr.sub_id WHERE s.name = @1 AND sr.privilege = 'ADMIN'", conn);
        command.Parameters.AddWithValue("1", sub);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            res.Add(reader.GetFieldValue<long>(0));
        }

        return res.ToArray();
    }

    public bool IsUserAdmin(long userId, string sub)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command =
            new NpgsqlCommand(
                "SELECT * FROM subs_rights sr "+
                "JOIN subs s ON s.id = sr.sub_id WHERE sr.user_id = @1 AND sr.privilege = 'Admin' AND s.name = @2", conn);
        command.Parameters.AddWithValue("1", userId);
        command.Parameters.AddWithValue("2", sub);
        var reader = command.ExecuteReader();
        return reader.HasRows;
    }

    public void CreateSubscription(string sub, long userId)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();

        using var command1 =
            new NpgsqlCommand(
                "INSERT INTO subs (name) VALUES (@1)", conn);
        command1.Parameters.AddWithValue("1", sub);
        command1.ExecuteNonQuery();
        using var command2 = new NpgsqlCommand("INSERT INTO subs_rights (user_id, sub_id, privilege) " +
                                               "VALUES (@1, (SELECT id FROM subs WHERE name = @2), @3)", conn);
        command2.Parameters.AddWithValue("1", userId);
        command2.Parameters.AddWithValue("2", sub);
        command2.Parameters.AddWithValue("3", "Admin");
        command2.ExecuteNonQuery();
    }

    public void DeleteSubscription(string sub, long userId)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        
        using var command1 = new NpgsqlCommand("DELETE FROM subs_rights " +
                                              "WHERE sub_id = (SELECT (id) FROM subs WHERE name = @1)", conn);
        command1.Parameters.AddWithValue("1", sub);
        command1.ExecuteNonQuery();
        using var command2 = new NpgsqlCommand("DELETE FROM subs " +
                                               "WHERE name = @1", conn);
        command2.Parameters.AddWithValue("1", sub);
        command2.ExecuteNonQuery();
    }

    public void SubscribeUser(long userId, string name)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command = new NpgsqlCommand("INSERT INTO subs_rights (user_id, sub_id, privilege)" +
                                              " VALUES (@1, (SELECT id FROM subs WHERE name = @2), @3)", conn);
        command.Parameters.AddWithValue("1", userId);
        command.Parameters.AddWithValue("2", name);
        command.Parameters.AddWithValue("3", "Follower");
        command.ExecuteNonQuery();
    }

    public void UnsubscribeUser(long userId, string name)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command = new NpgsqlCommand("DELETE FROM subs_rights " +
                                              " WHERE user_id = @1 AND sub_id = (SELECT id FROM subs WHERE name = @2)", conn);
        command.Parameters.AddWithValue("1", userId);
        command.Parameters.AddWithValue("2", name);
        command.ExecuteNonQuery();
    }
}