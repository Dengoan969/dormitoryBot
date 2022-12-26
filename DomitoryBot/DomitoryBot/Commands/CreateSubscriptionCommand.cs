using System.Text;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
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

        public DialogState DestinationState => throw new NotImplementedException();

        public async Task Execute(long chatId)
        {
            throw new NotImplementedException();
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                  "Введите описание объявления", Keyboard.Back);
        }
    }
}