namespace DomitoryBot.Domain;

public interface ISubscriptionRepository
{
    public long[] GetAllUsers(string name);
    public void AddAdmin(string sub, long caller, long userToAdd);
    public long[] GetAdmins(string name);
    public void SubscribeUser(long userId, string name);
    public void UnsubscribeUser(long userId, string name);
    public string[] GetSubscriptionsOfUser(long userId);
}