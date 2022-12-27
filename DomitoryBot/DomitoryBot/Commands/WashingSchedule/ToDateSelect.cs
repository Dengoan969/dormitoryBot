using DomitoryBot.Commands.Interfaces;
using Telegram;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands.WashingSchedule;

public class ToDateSelect : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public ToDateSelect(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Washing_Machine;
    public DialogState DestinationState => DialogState.Washing_Date;

    public async Task HandleMessage(Message message, long chatId)
    {
        if (dialogManager.Value.Schedule.machineNames.Contains(message.Text))
        {
            dialogManager.Value.temp_input[chatId] = new List<object>();
            dialogManager.Value.temp_input[chatId].Add(message.Text);
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                "Введите дату начала стирки в формате: число.месяц часы:минуты", Keyboard.Back);
        }
        else
        {
            await dialogManager.Value.ChangeState(SourceState, chatId, "Не знаю такую стиралку", Keyboard.Back);
        }
    }
}