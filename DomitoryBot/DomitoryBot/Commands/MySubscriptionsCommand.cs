using System.Text;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class MySubscriptionsCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public MySubscriptionsCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "MySubscriptions";

        public DialogState SourceState => DialogState.Subscriptions;

        public DialogState DestinationState => DialogState.Subscriptions;

        public async Task Execute(long chatId)
        {
            var subscriptions = dialogManager.Value.SubscriptionService.GetSubscriptionsOfUser(chatId);

            await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, String.Join("/n",subscriptions));

            await dialogManager.Value.ChangeState(DestinationState, chatId, "Подписки", Keyboard.Subscriptions);
        }
    }
}