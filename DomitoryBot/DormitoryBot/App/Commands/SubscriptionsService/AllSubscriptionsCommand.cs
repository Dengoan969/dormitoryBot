using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;
using DormitoryBot.App;
using DormitoryBot.Domain.SubscribitionService;

namespace DomitoryBot.App.Commands.SubscriptionsService
{
    public class AllSubscriptionsCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;
        private readonly SubscriptionService service;

        public AllSubscriptionsCommand(Lazy<IMessageSender> dialogManager, SubscriptionService service)
        {
            this.dialogManager = dialogManager;
            this.service = service;
        }

        public string Name => "AllSubscriptions";

        public DialogState SourceState => DialogState.Subscriptions;

        public DialogState DestinationState => DialogState.Subscriptions;

        public async Task Execute(long chatId)
        {
            var subscriptions = service.GetAllSubscriptions();
            if (subscriptions.Length == 0)
            {
                await dialogManager.Value.SendTextMessageAsync(chatId, "Пока нет никаких рассылок ._.");
            }
            else
            {
                await dialogManager.Value.SendTextMessageAsync(chatId,
                    $"Все рассылки:\n{string.Join("\n", subscriptions)}");
            }

            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Подписки", DestinationState);
        }
    }
}