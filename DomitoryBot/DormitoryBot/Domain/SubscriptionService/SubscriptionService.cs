namespace DormitoryBot.Domain.SubscriptionService
{
    public class SubscriptionService
    {
        private readonly ISubscriptionRepository repository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            this.repository = subscriptionRepository;
        }

        public string[] AllSubscriptions => repository.AllSubscriptions;

        public bool TrySubscribeUser(long userId, string sub)
        {
            var subs = repository.AllSubscriptions;
            sub = SubNameFormat(sub);
            if (subs.Contains(sub))
            {
                repository.SubscribeUser(userId, SubNameFormat(sub));
                return true;
            }
            return false;
        }

        public bool TryUnsubscribeUser(long userId, string sub)
        {
            var subs = repository.GetSubscriptionsOfUser(userId);
            sub = SubNameFormat(sub);
            if (subs.Contains(sub))
            {
                repository.UnsubscribeUser(userId, SubNameFormat(sub));
                return true;
            }
            return false;
        }

        public string[] GetSubscriptionsOfUser(long userId)
        {
            return repository.GetSubscriptionsOfUser(userId);
        }

        public string[] GetAdminSubscriptionsOfUser(long userId)
        {
            return repository.GetAdminSubscriptionsOfUser(userId);
        }

        // public void AddAdmin(string sub, long caller, long userToAdd)
        // {
        //     subscriptionRepository.AddAdmin(FormattedNameOfSub(sub), caller, userToAdd);
        // }

        public bool IsUserAdmin(long userId, string sub)
        {
            return repository.IsUserAdmin(userId, SubNameFormat(sub));
        }

        public bool TryCreateSubscription(string sub, long userId)
        {
            var subs = repository.AllSubscriptions;
            sub = SubNameFormat(sub);
            if (!subs.Contains(sub))
            {
                repository.CreateSubscription(SubNameFormat(sub), userId);
                return true;
            }

            return false;
        }

        public bool TryDeleteSubscription(string sub, long userId)
        {
            var subs = repository.GetAdminSubscriptionsOfUser(userId);
            sub = SubNameFormat(sub);
            if (subs.Contains(sub))
            {
                repository.DeleteSubscription(SubNameFormat(sub), userId);
                return true;
            }

            return false;
        }

        public List<long> GetFollowers(string sub)
        {
            return repository.GetFollowers(SubNameFormat(sub)).ToList();
        }

        private static string SubNameFormat(string sub)
        {
            if (sub.StartsWith("#")) return sub;

            return "#" + sub;
        }
    }
}