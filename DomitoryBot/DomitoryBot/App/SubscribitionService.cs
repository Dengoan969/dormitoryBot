using DomitoryBot.Infrastructure;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DomitoryBot.App
{
    public class SubscriptionService
    {
        private ISubscriptionRepository subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            this.subscriptionRepository = subscriptionRepository;
        }

        public bool SubscribeUser(long userId, string subscribName)
        {
            return subscriptionRepository.TrySubscribeUser(userId, subscribName);
        }

        public bool UnsubscribeUser(long userId, string subscribName)
        {
            return subscriptionRepository.TryUnsubscribeUser(userId, subscribName);
        }

        public string[] GetSubscriptionsOfUser(long userId)
        {
            return subscriptionRepository.GetSubscriptionsOfUser(userId);
        }

        public string[] GetAdminSubscriptionsOfUser(long userId)
        {
            return subscriptionRepository.GetAdminSubscriptionsOfUser(userId);
        }

        public void AddAdmin(string sub, long caller, long userToAdd)
        {
            subscriptionRepository.AddAdmin(sub, caller, userToAdd);
        }

        public bool IsUserAdmin(long userId, string sub)
        {
            return subscriptionRepository.IsUserAdmin(userId, sub);
        }

        public bool TryCreateSubscription(string sub, long userId)
        {
            return subscriptionRepository.TryCreateSubscription(sub, userId);
        }

        public bool TryDeleteSubscription(string sub, long userId)
        {
            return subscriptionRepository.TryDeleteSubscription(sub, userId);
        }

        public void SendAnnouncement(TelegramBotClient botClient, Message mes, string sub)
        {
            switch (mes.Type)
            {
                case MessageType.Photo:
                    {
                        foreach (var user in subscriptionRepository.GetFollowers(sub))
                            botClient.SendPhotoAsync(user, mes.Photo[0].FileId, $"{sub}\n{mes.Caption}");
                        break;
                    }

                case MessageType.Text:
                    foreach (var user in subscriptionRepository.GetFollowers(sub))
                        botClient.SendTextMessageAsync(user, $"{sub}\n{mes.Text}");
                    break;
            }
        }
    }
}