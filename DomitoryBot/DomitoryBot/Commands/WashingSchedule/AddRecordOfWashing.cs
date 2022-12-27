using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands.WashingSchedule;

public class AddRecordOfWashing : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dm;

    public AddRecordOfWashing(Lazy<DialogManager> dm)
    {
        this.dm = dm;
    }

    public DialogState SourceState => DialogState.Washing_Type;
    public DialogState DestinationState => DialogState.Washing;

    public async Task HandleMessage(Message message, long chatId)
    {
        if (int.TryParse(message.Text, out var num))
        {
            var washingTypes = dm.Value.Schedule.washingTypes;
            if (num < 1 || num > washingTypes.Count)
                await dm.Value.ChangeState(SourceState, chatId, "Неправильно указан номер типа стирки",
                    Keyboard.Back);

            var type = washingTypes.Keys.ToArray()[num - 1];
            var machine = dm.Value.temp_input[chatId][0] as string;
            var date = dm.Value.temp_input[chatId][1] as DateTime?;
            dm.Value.temp_input[chatId] = new List<object>();
            if (dm.Value.Schedule.TryAddRecord(chatId, machine, date.Value, type))
                await dm.Value.ChangeState(DestinationState, chatId, "Вы успешно записались на стирку",
                    Keyboard.Washing);
            else
                await dm.Value.ChangeState(DestinationState, chatId, "Это время уже занято",
                    Keyboard.Washing);
            return;
        }

        await dm.Value.ChangeState(SourceState, chatId, "Что то пошло не так", Keyboard.Back);
    }
}