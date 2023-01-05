using System.Globalization;
using System.Text;
using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.Domain.Schedule;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.WashingSchedule;

public class ToWashingTypeSelect : IHandleTextCommand
{
    private readonly Lazy<IDialogSender> dialogManager;
    private readonly Schedule schedule;

    public ToWashingTypeSelect(Lazy<IDialogSender> dialogManager, Schedule schedule)
    {
        this.dialogManager = dialogManager;
        this.schedule = schedule;
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
                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                    "Недопустимое время, вводите время кратное 30 минутам", SourceState);
            }
            else
            {
                dialogManager.Value.TempInput[chatId].Add(value);
                var sb = new StringBuilder();
                sb.Append("Отправьте мне номер типа стирки\n");
                var i = 1;
                foreach (var type in schedule.WashingTypes)
                {
                    sb.Append($"{i}) {type.Key} - {type.Value.Hours * 60 + type.Value.Minutes} минут\n");
                    i++;
                }


                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                    sb.ToString(), DestinationState);
            }

            return;
        }

        await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
            "Неверный формат времени", SourceState);
    }
}