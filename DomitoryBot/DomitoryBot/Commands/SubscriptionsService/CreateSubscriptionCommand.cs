using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;
using System.Text;
using Telegram.Bot;

namespace DomitoryBot.Commands.SubscriptionsService
{
    public class CreateSubscriptionCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public CreateSubscriptionCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "CreateSubscription";

        public DialogState SourceState => DialogState.Subscriptions_Manage;

        public DialogState DestinationState => DialogState.Subscriptions_Manage_Subscription;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                  "Как будет называться рассылка? (Для удобства лучше начинать название с #)", Keyboard.Back);
        }
    }
}