using DomitoryBot.Domain;
using Telegram;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands;

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
        if (Enum.TryParse<WashingType>(message.Text, out var type))
        {
            var machine = dm.Value.temp_input[chatId][0] as string;
            var date = dm.Value.temp_input[chatId][1] as DateTime?;
            if (dm.Value.Schedule.AddRecord(chatId, machine, date.Value, type))
                await dm.Value.ChangeState(DestinationState, chatId, "Вы успешно записались на стирку",
                    Keyboard.Washing);
            else
                await dm.Value.ChangeState(DestinationState, chatId, "Что то пошло не так. Попробуйте снова",
                    Keyboard.Washing);
        }

        await dm.Value.ChangeState(SourceState, chatId, "Что то пошло не так", Keyboard.Back);
    }
}