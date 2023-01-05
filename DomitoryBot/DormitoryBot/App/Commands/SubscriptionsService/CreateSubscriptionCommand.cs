using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DomitoryBot.App.Commands.SubscriptionsService
{
    public class CreateSubscriptionCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public CreateSubscriptionCommand(Lazy<IMessageSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "CreateSubscription";

        public DialogState SourceState => DialogState.SubscriptionsManage;

        public DialogState DestinationState => DialogState.SubscriptionsManageCreateSubscription;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Как будет называться рассылка? (Для удобства лучше начинать название с #)", DestinationState);
        }
    }
}