using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.WashingSchedule;

public class ToDateSelect : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public ToDateSelect(Lazy<DialogManager> dialogManager)
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
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                "Введите дату начала стирки в формате: число.месяц часы:минуты", Keyboard.Back);
        }
        else
        {
            await dialogManager.Value.ChangeState(SourceState, chatId, "Не знаю такую стиралку", Keyboard.Back);
        }
    }
}