using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.Marketplace
{
    public class HandleAdvertTextCommand : IHandleTextCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public HandleAdvertTextCommand(Lazy<TelegramDialogManager> dialogManager)
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
                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "Напиши что просишь/предложишь в награду", DestinationState, Keyboard.Back);
            }
            else
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "Напиши описание объявления", SourceState, Keyboard.Back);
            }
        }
    }
}