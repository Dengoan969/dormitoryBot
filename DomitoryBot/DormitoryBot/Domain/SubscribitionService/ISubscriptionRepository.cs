namespace DormitoryBot.Domain.SubscriptionService
{
    public interface ISubscriptionRepository
    {
        string[] AllSubscriptions { get; }

        string[] GetSubscriptionsOfUser(long userId);

        string[] GetAdminSubscriptionsOfUser(long userId);

        long[] GetFollowers(string sub);

        long[] GetAdmins(string sub);

        bool IsUserAdmin(long userId, string sub);

        void AddAdmin(string sub, long caller, long userToAdd);

        void CreateSubscription(string sub, long userId);

        void DeleteSubscription(string sub, long userId);

        void SubscribeUser(long userId, string name);

        void UnsubscribeUser(long userId, string name);
    }
}