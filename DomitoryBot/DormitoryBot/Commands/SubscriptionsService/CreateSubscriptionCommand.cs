using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.SubscriptionsService
{
    public class CreateSubscriptionCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public CreateSubscriptionCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "CreateSubscription";

        public DialogState SourceState => DialogState.SubscriptionsManage;

        public DialogState DestinationState => DialogState.SubscriptionsManageCreateSubscription;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                "Как будет называться рассылка? (Для удобства лучше начинать название с #)", Keyboard.Back);
        }
    }
}