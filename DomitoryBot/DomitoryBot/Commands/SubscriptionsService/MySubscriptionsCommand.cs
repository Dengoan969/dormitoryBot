using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;
using Telegram.Bot;

namespace DomitoryBot.Commands.SubscriptionsService
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
            if (subscriptions.Length == 0)
            {
                await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, "У тебя пока нет подписок ._.");
            }
            else
            {
                await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, $"Твои подписки:\n{string.Join("\n", subscriptions)}");
            }
            await dialogManager.Value.ChangeState(DestinationState, chatId, "Подписки", Keyboard.Subscriptions);
        }
    }
}