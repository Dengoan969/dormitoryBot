using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.Marketplace
{
    public class ToMarketplaceCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public ToMarketplaceCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Marketplace";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Marketplace;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Маркетплейс", DestinationState, Keyboard.Marketplace);
        }
    }
}