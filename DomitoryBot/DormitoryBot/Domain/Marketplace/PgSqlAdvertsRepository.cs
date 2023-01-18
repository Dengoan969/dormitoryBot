using System.Configuration;
using DormitoryBot.App.Interfaces;
using Npgsql;

namespace DormitoryBot.Domain.Marketplace;

public class PgSqlAdvertsRepository : IAdvertsRepository
{
    private static string connString = ConfigurationManager.ConnectionStrings["pgsql"].ConnectionString;
    public void AddAdvert(Advert advert)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command =
            new NpgsqlCommand(
                "INSERT INTO adverts (user_id, username, text, price, creation, ttl)" +
                "VALUES (@1, @2, @3, @4, @5, @6)");
        command.Parameters.AddWithValue("1", advert.Author);
        command.Parameters.AddWithValue("2", advert.Username);
        command.Parameters.AddWithValue("3", advert.Text);
        command.Parameters.AddWithValue("4", advert.Price);
        command.Parameters.AddWithValue("5", advert.CreationTime);
        command.Parameters.AddWithValue("6", advert.TimeToLive);
        command.ExecuteNonQuery();
    }

    public void RemoveAdvert(Advert advert)
    {
        using var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var command =
            new NpgsqlCommand("DELETE FROM adverts WHERE user_id = @1 AND creation = @2", conn);
        command.Parameters.AddWithValue("1", advert.Author);
        command.Parameters.AddWithValue("2", advert.CreationTime);
        command.ExecuteNonQuery();
    }

    public Advert[] Adverts
    {
        get
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var command =
                new NpgsqlCommand("SELECT user_id, username, text, price, creation, ttl FROM adverts");
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                
            }

            return Array.Empty<Advert>();
        }
    }

    public Advert[] GetUserAdverts(long user)
    {
        throw new NotImplementedException();
    }
}