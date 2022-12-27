using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;

namespace DomitoryBot.Commands.SubscriptionsService
{
    public class ToSubscriptionsCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;
        public string Name => "Subscriptions";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Subscriptions;

        public ToSubscriptionsCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId, "Объявления", Keyboard.Subscriptions);
        }
    }
}
