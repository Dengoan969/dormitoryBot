using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.Domain.SubscribitionService;

namespace DormitoryBot.Commands.SubscriptionsService
{
    public class MySubscriptionsCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;
        private readonly SubscriptionService service;

        public MySubscriptionsCommand(Lazy<IMessageSender> dialogManager, SubscriptionService service)
        {
            this.dialogManager = dialogManager;
            this.service = service;
        }

        public string Name => "MySubscriptions";

        public DialogState SourceState => DialogState.Subscriptions;

        public DialogState DestinationState => DialogState.Subscriptions;

        public async Task Execute(long chatId)
        {
            var subscriptions = service.GetSubscriptionsOfUser(chatId);
            if (subscriptions.Length == 0)
            {
                await dialogManager.Value.SendTextMessageAsync(chatId, "У тебя пока нет подписок ._.");
            }
            else
            {
                await dialogManager.Value.SendTextMessageAsync(chatId,
                    $"Твои подписки:\n{string.Join("\n", subscriptions)}");
            }

            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Подписки", DestinationState);
        }
    }
}