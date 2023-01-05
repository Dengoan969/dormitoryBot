using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DomitoryBot.App.Commands.WashingSchedule
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