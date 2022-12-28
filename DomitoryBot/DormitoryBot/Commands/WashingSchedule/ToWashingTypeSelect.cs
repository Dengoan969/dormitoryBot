using System.Globalization;
using System.Text;
using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.WashingSchedule;

public class ToWashingTypeSelect : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public ToWashingTypeSelect(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.WashingDate;
    public DialogState DestinationState => DialogState.WashingType;


    public async Task HandleMessage(Message message, long chatId)
    {
        if (DateTime.TryParseExact(message.Text, "d.M H:m", new CultureInfo("ru-RU"), DateTimeStyles.None,
                out var value))
        {
            if (value.Minute % 30 != 0)
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "Недопустимое время :(", SourceState, Keyboard.Back);
            }
            else
            {
                dialogManager.Value.TempInput[chatId].Add(value);
                var sb = new StringBuilder();
                sb.Append("Отправьте мне номер типа стирки\n");
                var i = 1;
                foreach (var type in dialogManager.Value.Schedule.WashingTypes)
                {
                    sb.Append($"{i}) {type.Key} - {type.Value.Hours * 60 + type.Value.Minutes} минут\n");
                    i++;
                }


                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    sb.ToString(), DestinationState, Keyboard.Back);
            }

            return;
        }

        await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
            "Неверный формат времени", SourceState, Keyboard.Back);
    }
}