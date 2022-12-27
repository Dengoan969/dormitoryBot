using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.SubscriptionsService
{
    public class DeleteSubscriptionCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public DeleteSubscriptionCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "DeleteSubscription";

        public DialogState SourceState => DialogState.Subscriptions_Manage;

        public DialogState DestinationState => DialogState.Subscriptions_Manage_Delete_Subscription;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                "Какую рассылку удалить?", Keyboard.Back);
        }
    }
}