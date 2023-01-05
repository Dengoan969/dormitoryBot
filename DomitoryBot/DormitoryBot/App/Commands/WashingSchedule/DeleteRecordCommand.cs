using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;
using DormitoryBot.App;
using DormitoryBot.Domain.Schedule;
using Telegram.Bot.Types;

namespace DomitoryBot.App.Commands.WashingSchedule;

public class DeleteRecordCommand : IHandleTextCommand
{
    private readonly Lazy<IMessageSender> dialogManager;
    private readonly Schedule schedule;

    public DeleteRecordCommand(Lazy<IMessageSender> dialogManager, Schedule schedule)
    {
        this.dialogManager = dialogManager;
        this.schedule = schedule;
    }

    public DialogState SourceState => DialogState.WashingSelectToDelete;
    public DialogState DestinationState => DialogState.Washing;

    public async Task HandleMessage(Message message, long chatId)
    {
        if (int.TryParse(message.Text, out var num))
        {
            var records = schedule.GetRecordsTimesByUser(chatId);
            if (num <= records.Count && num > 0)
            {
                schedule.TryRemoveRecord(records[num - 1]);
                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                    "Запись успешно удалена", DestinationState);
            }
            else
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                    "Вы ввели неправильное число", SourceState);
            }
        }
        else
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Вы ввели не число", SourceState);
        }
    }
}