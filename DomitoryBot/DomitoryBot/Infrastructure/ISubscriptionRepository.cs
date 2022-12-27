namespace DomitoryBot.Infrastructure;

public interface ISubscriptionRepository
{
    public long[] GetFollowers(string name);
    public void AddAdmin(string sub, long caller, long userToAdd);
    public long[] GetAdmins(string name);
    public bool IsUserAdmin(long userId, string sub);
    public bool TryCreateSubscription(string sub, long userId);
    public bool TryDeleteSubscription(string sub, long userId);
    public bool TrySubscribeUser(long userId, string name);
    public bool TryUnsubscribeUser(long userId, string name);
    public string[] GetSubscriptionsOfUser(long userId);
    public string[] GetAdminSubscriptionsOfUser(long userId);
}