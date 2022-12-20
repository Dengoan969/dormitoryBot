using Telegram;

namespace DomitoryBot.Commands;

public class ToMachineSelect : IExecutableCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public ToMachineSelect(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Washing;
    public DialogState DestinationState => DialogState.Washing_Machine;
    public string Name => "Machine select";

    public async Task Execute(long chatId)
    {
        await dialogManager.Value.ChangeState(DestinationState, chatId, "Выберите стиралку", Keyboard.Back);
    }
}