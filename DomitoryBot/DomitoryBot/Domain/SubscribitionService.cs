using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DomitoryBot.Domain
{
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
}