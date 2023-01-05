using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands.Marketplace
{
    public class CreateAdvertCommand : IExecutableCommand
    {
        private readonly Lazy<IDialogSender> dialogManager;

        public CreateAdvertCommand(Lazy<IDialogSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "CreateAdvert";

        public DialogState SourceState => DialogState.Marketplace;

        public DialogState DestinationState => DialogState.MarketplaceText;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Напиши описание объявления", DestinationState);
        }
    }
}