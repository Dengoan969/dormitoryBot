using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.Domain.Schedule;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.WashingSchedule;

public class ToDateSelect : IHandleTextCommand
{
    private readonly Lazy<TelegramDialogManager> dialogManager;
    private readonly Schedule schedule;

    public ToDateSelect(Lazy<TelegramDialogManager> dialogManager, Schedule schedule)
    {
        this.dialogManager = dialogManager;
        this.schedule = schedule;
    }

    public DialogState SourceState => DialogState.WashingMachine;
    public DialogState DestinationState => DialogState.WashingDate;

    public async Task HandleMessage(Message message, long chatId)
    {
        if (schedule.MachineNames.Contains(message.Text))
        {
            dialogManager.Value.TempInput[chatId] = new List<object>();
            dialogManager.Value.TempInput[chatId].Add(message.Text);
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Введите дату начала стирки в формате: число.месяц часы:минуты", DestinationState);
        }
        else
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Не знаю такую стиралку", SourceState);
        }
    }
}