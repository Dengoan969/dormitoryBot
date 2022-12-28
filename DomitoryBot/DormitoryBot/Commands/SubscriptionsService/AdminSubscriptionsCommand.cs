using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands.SubscriptionsService
{
    public class AdminSubscriptionsCommand : IExecutableCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public AdminSubscriptionsCommand(Lazy<TelegramDialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "AdminSubscriptions";

        public DialogState SourceState => DialogState.SubscriptionsManage;

        public DialogState DestinationState => DialogState.SubscriptionsManage;

        public async Task Execute(long chatId)
        {
            var subscriptions = dialogManager.Value.SubscriptionService.GetAdminSubscriptionsOfUser(chatId);
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