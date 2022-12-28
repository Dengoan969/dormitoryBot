using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.WashingSchedule;

public class ToDateSelect : IHandleTextCommand
{
    private readonly Lazy<TelegramDialogManager> dialogManager;

    public ToDateSelect(Lazy<TelegramDialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.WashingMachine;
    public DialogState DestinationState => DialogState.WashingDate;

    public async Task HandleMessage(Message message, long chatId)
    {
        if (dialogManager.Value.Schedule.MachineNames.Contains(message.Text))
        {
            dialogManager.Value.TempInput[chatId] = new List<object>();
            dialogManager.Value.TempInput[chatId].Add(message.Text);
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Введите дату начала стирки в формате: число.месяц часы:минуты", DestinationState, Keyboard.Back);
        }
        else
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Не знаю такую стиралку", SourceState, Keyboard.Back);
        }
    }
}