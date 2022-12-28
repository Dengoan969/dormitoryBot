using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.SubscriptionsService
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
                await dialogManager.Value.SendTextMessageAsync(chatId, "У тебя пока нет подписок ._.");
            }
            else
            {
                await dialogManager.Value.SendTextMessageAsync(chatId,
                    $"Твои подписки:\n{string.Join("\n", subscriptions)}");
            }

            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Подписки", DestinationState, Keyboard.Subscriptions);
        }
    }
}