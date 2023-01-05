using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DomitoryBot.App.Commands.Marketplace
{
    public class CreateAdvertCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public CreateAdvertCommand(Lazy<IMessageSender> dialogManager)
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