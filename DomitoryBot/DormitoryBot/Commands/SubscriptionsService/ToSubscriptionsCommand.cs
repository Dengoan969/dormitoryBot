using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands.SubscriptionsService
{
    public class ToSubscriptionsCommand : IExecutableCommand
    {
        private readonly Lazy<IDialogSender> dialogManager;

        public ToSubscriptionsCommand(Lazy<IDialogSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Subscriptions";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Subscriptions;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Объявления", DestinationState);
        }
    }
}