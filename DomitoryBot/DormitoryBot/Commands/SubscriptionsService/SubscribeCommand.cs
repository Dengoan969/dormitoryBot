using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.SubscriptionsService
{
    public class SubscribeCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public SubscribeCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Subscribe";

        public DialogState SourceState => DialogState.Subscriptions;

        public DialogState DestinationState => DialogState.SubscriptionsSubscribe;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "На какую рассылку подписаться?", DestinationState, Keyboard.Back);
        }
    }
}