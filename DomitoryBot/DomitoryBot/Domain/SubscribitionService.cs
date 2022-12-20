using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

        public bool SubscribeUser(long userId, string subscribName)
        {
            if (subscriptionRepository.GetSubscriptionsOfUser(userId).Contains(subscribName)) return false;

            subscriptionRepository.SubscribeUser(userId, subscribName);
            return true;
        }

        public bool UnsubscribeUser(long userId, string subscribName)
        {
            if (!subscriptionRepository.GetSubscriptionsOfUser(userId).Contains(subscribName)) return false;

            subscriptionRepository.UnsubscribeUser(userId, subscribName);
            return true;
        }

        public string[] GetSubscriptionsOfUser(long userId)
        {
            return subscriptionRepository.GetSubscriptionsOfUser(userId);
        }

        public void AddAdmin(string sub, long caller, long userToAdd)
        {
            subscriptionRepository.AddAdmin(sub, caller, userToAdd);
        }

        public bool IsUserAdmin(long userId, string sub)
        {
            return subscriptionRepository.GetAdmins(sub).Contains(userId);
        }

        public void SendAnnouncement(TelegramBotClient botClient, Message mes, string sub)
        {
            switch (mes.Type)
            {
                case MessageType.Photo:
                {
                    foreach (var user in subscriptionRepository.GetAllUsers(sub))
                        botClient.SendPhotoAsync(user, mes.Photo[0].FileId, mes.Caption);
                    break;
                }

                case MessageType.Text:
                    foreach (var user in subscriptionRepository.GetAllUsers(sub))
                        botClient.SendTextMessageAsync(user, mes.Text);
                    break;
            }
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
        public long[] GetAllUsers(string name);
        public void AddAdmin(string sub, long caller, long userToAdd);
        public long[] GetAdmins(string name);
        public void SubscribeUser(long userId, string name);
        public void UnsubscribeUser(long userId, string name);
        public string[] GetSubscriptionsOfUser(long userId);
    }

    public enum UserRights
    {
        Follower = 1,
        Admin = 2
    }

    public class MockSubscribitionRepository : ISubscriptionRepository
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
}