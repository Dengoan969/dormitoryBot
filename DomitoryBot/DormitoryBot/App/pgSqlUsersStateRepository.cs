using DormitoryBot.App.Interfaces;
using System.Configuration;
using Ninject.Parameters;
using Npgsql;

namespace DormitoryBot.App;

public class PgSqlUsersStateRepository : IUsersStateRepository
{
    private static string connString = ConfigurationManager.ConnectionStrings["pgsql"].ConnectionString;
    public DialogState GetState(long id)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command = new NpgsqlCommand("SELECT user_state FROM users WHERE user_id = @u", conn);
        command.Parameters.AddWithValue("u", id);
        var reader = command.ExecuteReader();
        reader.Read();
        var state = reader.GetValue(0).ToString();
        return Enum.Parse<DialogState>(state);

    }

    public bool ContainsKey(long id)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command = new NpgsqlCommand("SELECT user_id FROM users WHERE user_id = @u", conn);
        command.Parameters.AddWithValue("u", id);
        var r = command.ExecuteReader();
        return r.HasRows;
    }

    public void SetState(long id, DialogState state)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        var has_rows = false;
        using (var check_user_in_table = new NpgsqlCommand("SELECT * FROM users WHERE user_id = @u", conn))
        {
            check_user_in_table.Parameters.AddWithValue("u", id);
            var r = check_user_in_table.ExecuteReader();
            has_rows = r.HasRows;
            r.Close();
        }

        if (!has_rows)
        {
            using var command = new NpgsqlCommand("INSERT INTO users (user_id, user_state) VALUES (@u, @s)", conn);
            command.Parameters.AddWithValue("u", id);
            command.Parameters.AddWithValue("s", Enum.GetName(state));
            command.ExecuteNonQuery();
        }
        else
        {
            using var command = new NpgsqlCommand("UPDATE users SET user_state = @s WHERE user_id = @u", conn);
            command.Parameters.AddWithValue("u", id);
            command.Parameters.AddWithValue("s", Enum.GetName(state));
            command.ExecuteNonQuery();
        }
        
        
    }
}