namespace DomitoryBot.Domain;

public class MockSubscriptionRepository : ISubscriptionRepository
{
    private Dictionary<string, Dictionary<long, UserRights>> db;

    public long[] GetAllUsers(string name)
    {
        if (!db.ContainsKey(name)) throw new ArgumentException("No such subscription");

        return db[name].Keys.ToArray();
    }

    public void AddAdmin(string sub, long caller, long userToAdd)
    {
        if (!db.ContainsKey(sub)) throw new ArgumentException("No such subscription");

        if (db[sub].ContainsKey(caller) && db[sub][caller] == UserRights.Admin)
            db[sub][caller] = UserRights.Admin;
        else
            throw new ArgumentException($"Caller {caller} is not admin");
    }

    public long[] GetAdmins(string name)
    {
        if (!db.ContainsKey(name)) throw new ArgumentException("No such subscription");

        return db[name].Keys.Where(x => db[name][x] == UserRights.Admin).ToArray();
    }

    public void SubscribeUser(long userId, string name)
    {
        if (!db.ContainsKey(name)) throw new ArgumentException("No such subscription");

        if (db[name].ContainsKey(userId)) throw new ArgumentException("User in subscription");

        db[name][userId] = UserRights.Follower;
    }

    public void UnsubscribeUser(long userId, string name)
    {
        if (!db.ContainsKey(name)) throw new ArgumentException("No such subscription");

        if (!db[name].ContainsKey(userId)) throw new ArgumentException("User in subscription");

        db[name].Remove(userId);
    }

    public string[] GetSubscriptionsOfUser(long userId)
    {
        return db.Keys.Where(x => db[x].ContainsKey(userId)).ToArray();
    }
}