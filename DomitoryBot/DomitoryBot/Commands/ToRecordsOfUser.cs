using Telegram;

namespace DomitoryBot.Commands;

public class ToRecordsOfUser : IExecutableCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public ToRecordsOfUser(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState { get; }
    public DialogState DestinationState { get; }
    public string Name => "Delete record";

    public Task Execute(long chatId)
    {
        return Task.CompletedTask;
    }
}