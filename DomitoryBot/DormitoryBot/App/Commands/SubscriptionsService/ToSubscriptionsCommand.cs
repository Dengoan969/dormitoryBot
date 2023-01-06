using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DormitoryBot.App.Commands.SubscriptionsService
{
    public class ToSubscriptionsCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public ToSubscriptionsCommand(Lazy<IMessageSender> dialogManager)
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