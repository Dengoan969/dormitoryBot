namespace DomitoryBot.Domain
{
    public enum SubscriptionErrors
    {
        AlreadySubscribed,
        NotExistedSubscription
    }

    public class SubscriptionService
    {
        private ISubscriptionRepository subscriptionRepository;

        public bool SubscribeUser(Guid user, string subscribName)
        {
            if (subscriptionRepository.GetSubscriptionsOfUser(user).Contains(subscribName)) return false;

            subscriptionRepository.SubscribeUser(user, subscribName);
            return true;
        }

        public bool UnsubscribeUser(Guid user, string subscribName)
        {
            if (!subscriptionRepository.GetSubscriptionsOfUser(user).Contains(subscribName)) return false;

            subscriptionRepository.UnsubscribeUser(user, subscribName);
            return true;
        }

        public void MakeAnnouncement(string topic, string text, string type)
        {
            var ann = new Announcement(topic, text, type);
            SendAnnouncement(ann);
        }

        private void SendAnnouncement(Announcement announcement)
        {
            throw new NotImplementedException();
        }
    }

    public class Announcement
    {
        public readonly string Text;
        public readonly string Topic;
        public readonly string Type;

        public Announcement(string topic, string text, string type)
        {
            Topic = topic;
            Text = text;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Topic}\n{Text}\n{Type}";
        }
    }

    public interface ISubscriptionRepository
    {
        public Guid[] GetAllUsers(string name);
        public Guid[] GetAdmins(string name);
        public void SubscribeUser(Guid user, string name);
        public void UnsubscribeUser(Guid user, string name);
        public string[] GetSubscriptionsOfUser(Guid user);
    }
}