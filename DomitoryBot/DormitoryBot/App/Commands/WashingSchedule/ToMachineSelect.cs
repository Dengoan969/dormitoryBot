using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DormitoryBot.App.Commands.WashingSchedule;

public class ToMachineSelect : IExecutableCommand
{
    private readonly Lazy<IMessageSender> dialogManager;

    public ToMachineSelect(Lazy<IMessageSender> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Washing;
    public DialogState DestinationState => DialogState.WashingMachine;
    public string Name => "Machine select";

    public async Task Execute(long chatId)
    {
        await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
            "Напишите номер стиралки", DestinationState);
    }
}