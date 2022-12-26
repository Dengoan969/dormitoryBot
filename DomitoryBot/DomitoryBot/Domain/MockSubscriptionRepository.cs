﻿namespace DomitoryBot.Domain;

public class MockSubscriptionRepository : ISubscriptionRepository
{
    private Dictionary<string, Dictionary<long, UserRights>> db = new Dictionary<string, Dictionary<long, UserRights>>();

    public long[] GetFollowers(string name)
    {
        return db[name].Keys.Where(x => db[name][x] == UserRights.Follower).ToArray();
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
    public bool TryCreateSubscription(string sub, long userId)
    {
        if (db.ContainsKey(sub))
        {
            return false;
        }
        db[sub] = new Dictionary<long, UserRights>() { { userId, UserRights.Admin } };
        return true;
    }

    public bool TrySubscribeUser(long userId, string name)
    {
        if (!db.ContainsKey(name) || db[name].ContainsKey(userId))
        {
            return false;
        }
        db[name][userId] = UserRights.Follower;
        return true;
    }

    public bool TryUnsubscribeUser(long userId, string name)
    {
        if (!db.ContainsKey(name) || !db[name].ContainsKey(userId))
        {
            return false;
        }
        db[name].Remove(userId);
        return true;
    }

    public string[] GetSubscriptionsOfUser(long userId)
    {
        return db.Keys.Where(x => db[x].ContainsKey(userId) && db[x][userId] == UserRights.Follower).ToArray();
    }
    public string[] GetAdminSubscriptionsOfUser(long userId)
    {
        return db.Keys.Where(x => db[x].ContainsKey(userId) && db[x][userId] == UserRights.Admin).ToArray();
    }
}