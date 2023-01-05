using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DomitoryBot.App.Commands.Marketplace
{
    public class ToMarketplaceCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public ToMarketplaceCommand(Lazy<IMessageSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Marketplace";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Marketplace;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Маркетплейс", DestinationState);
        }
    }
}