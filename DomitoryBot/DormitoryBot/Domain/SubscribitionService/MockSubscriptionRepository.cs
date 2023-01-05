using DomitoryBot.Domain.SubscribitionService;

namespace DormitoryBot.Domain.SubscribitionService;

public class MockSubscriptionRepository : ISubscriptionRepository
{
    private readonly Dictionary<string, Dictionary<long, UserRights>> db;

    public MockSubscriptionRepository(Dictionary<string, Dictionary<long, UserRights>> db)
    {
        this.db = db;
    }

    public MockSubscriptionRepository() : this(new Dictionary<string, Dictionary<long, UserRights>>()) { }

    public long[] GetFollowers(string name)
    {
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
        if (!db.ContainsKey(name))
        {
            return new long[0];
        }

        return db[name].Keys.Where(x => db[name][x] == UserRights.Admin).ToArray();
    }

    public bool IsUserAdmin(long userId, string sub)
    {
        return GetAdmins(sub).Contains(userId);
    }

    public void CreateSubscription(string sub, long userId)
    {
        db[sub] = new Dictionary<long, UserRights> { { userId, UserRights.Admin } };
    }

    public void DeleteSubscription(string sub, long userId)
    {
        db.Remove(sub);
    }

    public void SubscribeUser(long userId, string name)
    {
        db[name][userId] = UserRights.Follower;
    }

    public void UnsubscribeUser(long userId, string name)
    {
        db[name].Remove(userId);
    }

    public string[] GetSubscriptionsOfUser(long userId)
    {
        return db.Keys.Where(x => db[x].ContainsKey(userId) && db[x][userId] == UserRights.Follower).ToArray();
    }

    public string[] GetAdminSubscriptionsOfUser(long userId)
    {
        return db.Keys.Where(x => db[x].ContainsKey(userId) && db[x][userId] == UserRights.Admin).ToArray();
    }

    public string[] GetAllSubscriptions()
    {
        return db.Keys.ToArray();
    }
}