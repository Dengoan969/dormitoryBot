using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.WashingSchedule;

public class AddRecordOfWashing : IHandleTextCommand
{
    private readonly Lazy<TelegramDialogManager> dm;

    public AddRecordOfWashing(Lazy<TelegramDialogManager> dm)
    {
        this.dm = dm;
    }

    public DialogState SourceState => DialogState.WashingType;
    public DialogState DestinationState => DialogState.Washing;

    public async Task HandleMessage(Message message, long chatId)
    {
        if (int.TryParse(message.Text, out var num))
        {
            var washingTypes = dm.Value.Schedule.WashingTypes;
            if (num < 1 || num > washingTypes.Count)
                await dm.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "Неправильно указан номер типа стирки", SourceState);

            var type = washingTypes.Keys.ToArray()[num - 1];
            var machine = dm.Value.TempInput[chatId][0] as string;
            var date = dm.Value.TempInput[chatId][1] as DateTime?;
            dm.Value.TempInput[chatId] = new List<object>();
            if (dm.Value.Schedule.TryAddRecord(chatId, machine, date.Value, type))
                await dm.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "Вы успешно записались на стирку", DestinationState);
            else
                await dm.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "Это время уже занято", DestinationState);
            return;
        }

        await dm.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId, "Что то пошло не так", SourceState);
    }
}