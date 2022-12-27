using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;

namespace DomitoryBot.Commands.WashingSchedule;

public class ChooseDaysFreeSlotsCommand : IExecutableCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public ChooseDaysFreeSlotsCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public string Name => "FreeSlots";

    public DialogState SourceState => DialogState.Washing;

    public DialogState DestinationState => DialogState.Washing_FreeSlots_ChooseDays;

    public async Task Execute(long chatId)
    {
        await dialogManager.Value.ChangeState(DestinationState,
            chatId, "Введите интересующий день в формате день.месяц", Keyboard.Back);
    }
}