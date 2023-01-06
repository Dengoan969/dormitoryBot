using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DormitoryBot.App.Commands.SubscriptionsService
{
    public class DeleteSubscriptionCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public DeleteSubscriptionCommand(Lazy<IMessageSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "DeleteSubscription";

        public DialogState SourceState => DialogState.SubscriptionsManage;

        public DialogState DestinationState => DialogState.SubscriptionsManageDeleteSubscription;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Какую рассылку удалить?", DestinationState);
        }
    }
}