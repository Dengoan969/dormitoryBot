using System.Globalization;
using System.Text;
using DomitoryBot.Domain;
using Telegram;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands;

public class ToWashingTypeSelect : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public ToWashingTypeSelect(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Washing_Date;
    public DialogState DestinationState => DialogState.Washing_Type;


    public async Task HandleMessage(Message message, long chatId)
    {
        if (DateTime.TryParseExact(message.Text, "dd.MM HH:mm", new CultureInfo("ruRU"), DateTimeStyles.None,
                out var value))
        {
            value = value.AddYears(DateTime.Today.Year);
            dialogManager.Value.temp_input[chatId].Add(value);
            var sb = new StringBuilder();
            sb.Append("Выберите тип стирки\n");
            foreach (var types in Schedule.washingTypes)
                sb.Append($"{Enum.GetName(types.Key)} - {types.Value.Minutes} минут\n");

            await dialogManager.Value.ChangeState(DestinationState, chatId, sb.ToString(), Keyboard.Back);
            return;
        }

        await dialogManager.Value.ChangeState(SourceState, chatId, "Неверный формат времени", Keyboard.Back);
    }
}