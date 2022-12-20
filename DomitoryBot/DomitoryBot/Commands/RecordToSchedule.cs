using Telegram;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands;

public class RecordToSchedule : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public RecordToSchedule(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Washing_Date;
    public DialogState DestinationState => DialogState.Washing;


    public async Task HandleMessage(Message message, long chatId)
    {
        DateTime.TryParseExact(message.Text, "dd.MM HH:mm")
        {
        }
    }
}