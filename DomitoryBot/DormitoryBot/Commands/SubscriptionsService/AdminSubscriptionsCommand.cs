using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.SubscriptionsService
{
    public class AdminSubscriptionsCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public AdminSubscriptionsCommand(Lazy<DialogManager> dialogManager)
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

            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Управление рассылками", DestinationState, Keyboard.SubscriptionsManage);
        }
    }
}