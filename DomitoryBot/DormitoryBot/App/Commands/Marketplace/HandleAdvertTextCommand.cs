using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;
using Telegram.Bot.Types;

namespace DormitoryBot.App.Commands.Marketplace
{
    public class HandleAdvertTextCommand : IHandleTextCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public HandleAdvertTextCommand(Lazy<IMessageSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public DialogState SourceState => DialogState.MarketplaceText;
        public DialogState DestinationState => DialogState.MarketplacePrice;


        public async Task HandleMessage(Message message, long chatId)
        {
            if (message.Text != null)
            {
                dialogManager.Value.TempInput[chatId] = new List<object>();
                dialogManager.Value.TempInput[chatId].Add(message.Text);
                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                    "Напиши что просишь/предложишь в награду", DestinationState);
            }
            else
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                    "Напиши описание объявления", SourceState);
            }
        }
    }
}