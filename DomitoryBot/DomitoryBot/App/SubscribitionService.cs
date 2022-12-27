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

        public bool SubscribeUser(long userId, string sub)
        {
            return subscriptionRepository.TrySubscribeUser(userId, FormattedNameOfSub(sub));
        }

        public bool UnsubscribeUser(long userId, string sub)
        {
            return subscriptionRepository.TryUnsubscribeUser(userId, FormattedNameOfSub(sub));
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
            subscriptionRepository.AddAdmin(FormattedNameOfSub(sub), caller, userToAdd);
        }

        public bool IsUserAdmin(long userId, string sub)
        {
            return subscriptionRepository.IsUserAdmin(userId, FormattedNameOfSub(sub));
        }

        public bool TryCreateSubscription(string sub, long userId)
        {
            return subscriptionRepository.TryCreateSubscription(FormattedNameOfSub(sub), userId);
        }

        public bool TryDeleteSubscription(string sub, long userId)
        {
            return subscriptionRepository.TryDeleteSubscription(FormattedNameOfSub(sub), userId);
        }

        public void SendAnnouncement(TelegramBotClient botClient, Message mes, string sub)
        {
            ;
            switch (mes.Type)
            {
                case MessageType.Photo:
                {
                    foreach (var user in subscriptionRepository.GetFollowers(FormattedNameOfSub(sub)))
                        botClient.SendPhotoAsync(user, mes.Photo[0].FileId,
                            $"{FormattedNameOfSub(sub)}\n{mes.Caption}");
                    break;
                }

                case MessageType.Text:
                    foreach (var user in subscriptionRepository.GetFollowers(FormattedNameOfSub(sub)))
                        botClient.SendTextMessageAsync(user, $"{FormattedNameOfSub(sub)}\n{mes.Text}");
                    break;
            }
        }

        private string FormattedNameOfSub(string sub)
        {
            if (sub.StartsWith("#")) return sub;

            return "#" + sub;
        }
    }
}