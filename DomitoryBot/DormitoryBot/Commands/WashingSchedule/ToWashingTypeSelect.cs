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

    public DialogState SourceState => DialogState.Washing_Date;
    public DialogState DestinationState => DialogState.Washing_Type;


    public async Task HandleMessage(Message message, long chatId)
    {
        if (DateTime.TryParseExact(message.Text, "d.M H:m", new CultureInfo("ru-RU"), DateTimeStyles.None,
                out var value))
        {
            if (value.Minute % 30 != 0)
            {
                await dialogManager.Value.ChangeState(SourceState, chatId, "Недопустимое время :(", Keyboard.Back);
            }
            else
            {
                dialogManager.Value.temp_input[chatId].Add(value);
                var sb = new StringBuilder();
                sb.Append("Отправьте мне номер типа стирки\n");
                var i = 1;
                foreach (var type in dialogManager.Value.Schedule.washingTypes)
                {
                    sb.Append($"{i}) {type.Key} - {type.Value.Hours * 60 + type.Value.Minutes} минут\n");
                    i++;
                }


                await dialogManager.Value.ChangeState(DestinationState, chatId, sb.ToString(), Keyboard.Back);
            }

            return;
        }

        await dialogManager.Value.ChangeState(SourceState, chatId, "Неверный формат времени", Keyboard.Back);
    }
}