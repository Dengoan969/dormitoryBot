using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.WashingSchedule;

public class ToMachineSelect : IExecutableCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public ToMachineSelect(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Washing;
    public DialogState DestinationState => DialogState.WashingMachine;
    public string Name => "Machine select";

    public async Task Execute(long chatId)
    {
        await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
            "Напишите номер стиралки", DestinationState, Keyboard.Back);
    }
}