using DomitoryBot.Domain.SubscribitionService;

namespace DormitoryBot.Domain.SubscribitionService
{
    public class SubscriptionService
    {
        private ISubscriptionRepository subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            this.subscriptionRepository = subscriptionRepository;
        }

        public bool TrySubscribeUser(long userId, string sub)
        {
            var subs = subscriptionRepository.GetAllSubscriptions();
            sub = FormattedNameOfSub(sub);
            if (subs.Contains(sub))
            {
                subscriptionRepository.SubscribeUser(userId, FormattedNameOfSub(sub));
                return true;
            }

            return false;
        }

        public bool UnsubscribeUser(long userId, string sub)
        {
            var subs = subscriptionRepository.GetSubscriptionsOfUser(userId);
            sub = FormattedNameOfSub(sub);
            if (subs.Contains(sub))
            {
                subscriptionRepository.UnsubscribeUser(userId, FormattedNameOfSub(sub));
                return true;
            }

            return false;
        }

        public string[] GetSubscriptionsOfUser(long userId)
        {
            return subscriptionRepository.GetSubscriptionsOfUser(userId);
        }

        public string[] GetAdminSubscriptionsOfUser(long userId)
        {
            return subscriptionRepository.GetAdminSubscriptionsOfUser(userId);
        }

        // public void AddAdmin(string sub, long caller, long userToAdd)
        // {
        //     subscriptionRepository.AddAdmin(FormattedNameOfSub(sub), caller, userToAdd);
        // }

        public bool IsUserAdmin(long userId, string sub)
        {
            return subscriptionRepository.IsUserAdmin(userId, FormattedNameOfSub(sub));
        }

        public bool TryCreateSubscription(string sub, long userId)
        {
            var subs = subscriptionRepository.GetAllSubscriptions();
            sub = FormattedNameOfSub(sub);
            if (!subs.Contains(sub))
            {
                subscriptionRepository.CreateSubscription(FormattedNameOfSub(sub), userId);
                return true;
            }

            return false;
        }

        public bool TryDeleteSubscription(string sub, long userId)
        {
            var subs = subscriptionRepository.GetAdminSubscriptionsOfUser(userId);
            sub = FormattedNameOfSub(sub);
            if (subs.Contains(sub))
            {
                subscriptionRepository.DeleteSubscription(FormattedNameOfSub(sub), userId);
                return true;
            }

            return false;
        }

        public List<long> GetFollowers(string sub)
        {
            return subscriptionRepository.GetFollowers(FormattedNameOfSub(sub)).ToList();
        }

        private string FormattedNameOfSub(string sub)
        {
            if (sub.StartsWith("#")) return sub;

            return "#" + sub;
        }
    }
}