using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DomitoryBot.App.Commands.SubscriptionsService
{
    public class UnsubscribeCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public UnsubscribeCommand(Lazy<IMessageSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Unsubscribe";

        public DialogState SourceState => DialogState.Subscriptions;

        public DialogState DestinationState => DialogState.SubscriptionsUnsubscribe;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "От какой рассылки отписаться?", DestinationState);
        }
    }
}