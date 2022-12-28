using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.Marketplace
{
    public class HandleAdvertPriceCommand : IHandleTextCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public HandleAdvertPriceCommand(Lazy<TelegramDialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public DialogState SourceState => DialogState.MarketplacePrice;
        public DialogState DestinationState => DialogState.MarketplaceTime;


        public async Task HandleMessage(Message message, long chatId)
        {
            if (message.Text != null)
            {
                dialogManager.Value.TempInput[chatId].Add(message.Text);
                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "На сколько дней разместить объявление?", DestinationState, Keyboard.Back);
            }
            else
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "Напиши что просишь/предложишь в награду", SourceState, Keyboard.Back);
            }
        }
    }
}