using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands.WashingSchedule;

public class ChooseDaysFreeSlotsCommand : IExecutableCommand
{
    private readonly Lazy<IDialogSender> dialogManager;

    public ChooseDaysFreeSlotsCommand(Lazy<IDialogSender> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public string Name => "FreeSlots";

    public DialogState SourceState => DialogState.Washing;

    public DialogState DestinationState => DialogState.WashingFreeSlotsChooseDays;

    public async Task Execute(long chatId)
    {
        await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
            "Доступны дни: сегодня, завтра, послезавтра.\nВведите интересующий день в формате день.месяц.",
            DestinationState);
    }
}