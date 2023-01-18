using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;
using Telegram.Bot.Types;

namespace DormitoryBot.App.Commands.Marketplace
{
    public class HandleAdvertPriceCommand : IHandleTextCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public HandleAdvertPriceCommand(Lazy<IMessageSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public DialogState SourceState => DialogState.MarketplacePrice;
        public DialogState DestinationState => DialogState.MarketplaceTime;


        public async Task HandleMessage(ChatMessage message, long chatId)
        {
            if (message.Text != null)
            {
                dialogManager.Value.TempInput[chatId].Add(message.Text);
                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                    "На сколько дней разместить объявление?", DestinationState);
            }
            else
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                    "Напиши что просишь/предложишь в награду", SourceState);
            }
        }
    }
}