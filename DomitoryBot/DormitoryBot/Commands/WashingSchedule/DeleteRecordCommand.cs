using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.WashingSchedule;

public class DeleteRecordCommand : IHandleTextCommand
{
    private readonly Lazy<TelegramDialogManager> dialogManager;

    public DeleteRecordCommand(Lazy<TelegramDialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.WashingSelectToDelete;
    public DialogState DestinationState => DialogState.Washing;

    public async Task HandleMessage(Message message, long chatId)
    {
        if (int.TryParse(message.Text, out var num))
        {
            var records = dialogManager.Value.Schedule.GetRecordsTimesByUser(chatId);
            if (num <= records.Count && num > 0)
            {
                dialogManager.Value.Schedule.TryRemoveRecord(records[num - 1]);
                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "Запись успешно удалена", DestinationState, Keyboard.Washing);
            }
            else
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "Вы ввели неправильное число", SourceState, Keyboard.Back);
            }
        }
        else
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Вы ввели не число", SourceState, Keyboard.Back);
        }
    }
}