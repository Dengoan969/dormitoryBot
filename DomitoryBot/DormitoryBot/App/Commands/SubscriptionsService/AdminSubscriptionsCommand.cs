using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;
using DormitoryBot.App;
using DormitoryBot.Domain.SubscribitionService;

namespace DomitoryBot.App.Commands.SubscriptionsService
{
    public class AdminSubscriptionsCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;
        private readonly SubscriptionService service;

        public AdminSubscriptionsCommand(Lazy<IMessageSender> dialogManager, SubscriptionService service)
        {
            this.dialogManager = dialogManager;
            this.service = service;
        }

        public string Name => "AdminSubscriptions";

        public DialogState SourceState => DialogState.SubscriptionsManage;

        public DialogState DestinationState => DialogState.SubscriptionsManage;

        public async Task Execute(long chatId)
        {
            var subscriptions = service.GetAdminSubscriptionsOfUser(chatId);
            if (subscriptions.Length == 0)
            {
                await dialogManager.Value.SendTextMessageAsync(chatId,
                    "У тебя пока нет созданных рассылок ._.");
            }
            else
            {
                await dialogManager.Value.SendTextMessageAsync(chatId,
                    $"Твои рассылки:\n{string.Join("\n", subscriptions)}");
            }

            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Управление рассылками", DestinationState);
        }
    }
}