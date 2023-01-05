namespace DomitoryBot.Domain.SubscribitionService;

public interface ISubscriptionRepository
{
    public long[] GetFollowers(string name);
    public void AddAdmin(string sub, long caller, long userToAdd);
    public long[] GetAdmins(string name);
    public bool IsUserAdmin(long userId, string sub);
    public void CreateSubscription(string sub, long userId);
    public void DeleteSubscription(string sub, long userId);
    public void SubscribeUser(long userId, string name);
    public void UnsubscribeUser(long userId, string name);
    public string[] GetSubscriptionsOfUser(long userId);
    public string[] GetAdminSubscriptionsOfUser(long userId);

    string[] GetAllSubscriptions();
}