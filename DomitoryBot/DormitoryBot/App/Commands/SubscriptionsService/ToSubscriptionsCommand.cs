using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DomitoryBot.App.Commands.SubscriptionsService
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