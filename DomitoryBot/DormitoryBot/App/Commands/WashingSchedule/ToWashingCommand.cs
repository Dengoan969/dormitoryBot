using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DormitoryBot.App.Commands.WashingSchedule
{
    public class ToWashingCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public ToWashingCommand(Lazy<IMessageSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Washing";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Washing;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId, "Стирка",
                DestinationState);
        }
    }
}