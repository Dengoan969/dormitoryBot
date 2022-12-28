using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.Marketplace
{
    public class HandleAdvertTimeCommand : IHandleTextCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public HandleAdvertTimeCommand(Lazy<TelegramDialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public DialogState SourceState => DialogState.MarketplaceTime;
        public DialogState DestinationState => DialogState.Marketplace;


        public async Task HandleMessage(Message message, long chatId)
        {
            if (message.Text != null)
            {
                var tempInput = dialogManager.Value.TempInput[chatId];
                if (!int.TryParse(message.Text, out var days))
                {
                    await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                        "Кажется ты вводишь что-то не то.. Необходимо целое число дней", SourceState, Keyboard.Back);
                }

                if (days <= 0)
                {
                    await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                        "Попробуй положительное число :)", SourceState, Keyboard.Back);
                }
                else if (days > 30)
                {
                    await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                        "Слишком много, давай не больше 30", SourceState, Keyboard.Back);
                }
                else
                {
                    dialogManager.Value.MarketPlace.CreateAdvert(chatId, (string) tempInput[0], (string) tempInput[1],
                        TimeSpan.FromDays(days), message.From.Username);
                    await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                        "Маркетплейс", DestinationState, Keyboard.Marketplace);
                }
            }
            else
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "На сколько дней разместить объявление?", SourceState, Keyboard.Back);
            }
        }
    }
}