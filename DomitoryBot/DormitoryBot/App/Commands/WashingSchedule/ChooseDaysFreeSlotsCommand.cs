using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DormitoryBot.App.Commands.WashingSchedule;

public class ChooseDaysFreeSlotsCommand : IExecutableCommand
{
    private readonly Lazy<IMessageSender> dialogManager;

    public ChooseDaysFreeSlotsCommand(Lazy<IMessageSender> dialogManager)
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