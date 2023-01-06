using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;
using DormitoryBot.Domain.Marketplace;
using Telegram.Bot.Types;

namespace DormitoryBot.App.Commands.Marketplace
{
    public class HandleAdvertTimeCommand : IHandleTextCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;
        private readonly MarketPlace marketPlace;

        public HandleAdvertTimeCommand(Lazy<IMessageSender> dialogManager, MarketPlace marketPlace)
        {
            this.dialogManager = dialogManager;
            this.marketPlace = marketPlace;
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
                    await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                        "Кажется ты вводишь что-то не то.. Необходимо целое число дней", SourceState);
                    return;
                }

                if (days <= 0)
                {
                    await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                        "Попробуй положительное число :)", SourceState);
                }
                else if (days > 30)
                {
                    await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                        "Слишком много, давай не больше 30", SourceState);
                }
                else
                {
                    marketPlace.CreateAdvert(chatId, (string)tempInput[0], (string)tempInput[1],
                        TimeSpan.FromDays(days), message.From.Username);
                    await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                        "Маркетплейс", DestinationState);
                }
            }
            else
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                    "На сколько дней разместить объявление?", SourceState);
            }
        }
    }
}