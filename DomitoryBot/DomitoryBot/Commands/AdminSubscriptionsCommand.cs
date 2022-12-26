using System.Text;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class AdminSubscriptionsCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public AdminSubscriptionsCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "AdminSubscriptions";

        public DialogState SourceState => DialogState.Subscriptions_Manage;

        public DialogState DestinationState => DialogState.Subscriptions_Manage;

        public async Task Execute(long chatId)
        {
            var subscriptions = dialogManager.Value.SubscriptionService.GetSubscriptionsOfUser(chatId);

            await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, String.Join("/n",subscriptions));

            await dialogManager.Value.ChangeState(DestinationState, chatId, "Управление подписками", Keyboard.SubscriptionsManage);
        }
    }
}