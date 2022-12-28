using System.Text;
using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.WashingSchedule;

public class ToRecordsOfUser : IExecutableCommand
{
    private readonly Lazy<TelegramDialogManager> dialogManager;

    public ToRecordsOfUser(Lazy<TelegramDialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Washing;
    public DialogState DestinationState => DialogState.WashingSelectToDelete;
    public string Name => "Delete record";

    public async Task Execute(long chatId)
    {
        var records = dialogManager.Value.Schedule.GetRecordsTimesByUser(chatId);

        if (records.Count == 0)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "У вас нет записей", SourceState, Keyboard.Washing);
        }
        else
        {
            var sb = new StringBuilder();
            sb.Append("Ваши записи:\n");
            for (var i = 0; i < records.Count; i++)
                sb.Append($"{i + 1}. {records[i].TimeInterval.Start.ToString("dd.MM HH:mm")}" +
                          $" - {records[i].TimeInterval.End.ToString("dd.MM HH:mm")}" +
                          $" Номер машинки:{records[i].Machine}\n");
            await dialogManager.Value.SendTextMessageAsync(chatId, "Введите номер записи для удаления");
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                sb.ToString(), DestinationState, Keyboard.Back);
        }
    }
}