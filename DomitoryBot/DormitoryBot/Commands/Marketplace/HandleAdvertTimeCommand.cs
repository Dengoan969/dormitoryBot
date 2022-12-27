using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.Marketplace
{
    public class HandleAdvertTimeCommand : IHandleTextCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public HandleAdvertTimeCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public DialogState SourceState => DialogState.Marketplace_Time;
        public DialogState DestinationState => DialogState.Marketplace;


        public async Task HandleMessage(Message message, long chatId)
        {
            if (message.Text != null)
            {
                var temp_input = dialogManager.Value.temp_input[chatId];
                if (!int.TryParse(message.Text, out var days))
                {
                    await dialogManager.Value.ChangeState(SourceState, chatId,
                        "Кажется ты вводишь что-то не то.. Необходимо целое число дней", Keyboard.Back);
                }

                if (days <= 0)
                {
                    await dialogManager.Value.ChangeState(SourceState, chatId,
                        "Попробуй положительное число :)", Keyboard.Back);
                }
                else if (days > 30)
                {
                    await dialogManager.Value.ChangeState(SourceState, chatId,
                        "Слишком много, давай не больше 30", Keyboard.Back);
                }
                else
                {
                    dialogManager.Value.MarketPlace.CreateAdvert(chatId, (string) temp_input[0], (string) temp_input[1],
                        TimeSpan.FromDays(days), message.From.Username);
                    await dialogManager.Value.ChangeState(DestinationState, chatId,
                        "Маркетплейс", Keyboard.Marketplace);
                }
            }
            else
            {
                await dialogManager.Value.ChangeState(SourceState, chatId,
                    "На сколько дней разместить объявление?", Keyboard.Back);
            }
        }
    }
}