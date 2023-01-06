using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DormitoryBot.App.Commands.SubscriptionsService
{
    public class SubscribeCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public SubscribeCommand(Lazy<IMessageSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Subscribe";

        public DialogState SourceState => DialogState.Subscriptions;

        public DialogState DestinationState => DialogState.SubscriptionsSubscribe;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "На какую рассылку подписаться?", DestinationState);
        }
    }
}