using Telegram;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands;

public class DeleteRecordCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public DeleteRecordCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Washing_SelectToDelete;
    public DialogState DestinationState => DialogState.Washing;

    public async Task HandleMessage(Message message, long chatId)
    {
        if (int.TryParse(message.Text, out var num))
        {
            var records = dialogManager.Value.Schedule.GetRecordsTimesByUser(chatId);
            if (num <= records.Count && num > 0)
            {
                dialogManager.Value.Schedule.RemoveRecord(records[num - 1]);
                await dialogManager.Value.ChangeState(DestinationState, chatId, "Запись успешно удалена",
                    Keyboard.Washing);
            }
            else
            {
                await dialogManager.Value.ChangeState(SourceState, chatId, "Вы ввели неправильное число",
                    Keyboard.Back);
            }
        }
        else
        {
            await dialogManager.Value.ChangeState(SourceState, chatId, "Вы ввели не число", Keyboard.Back);
        }
    }
}