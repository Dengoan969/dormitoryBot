using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.WashingSchedule
{
    public class ToWashingCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public ToWashingCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Washing";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Washing;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId, "Стирка",
                DestinationState, Keyboard.Washing);
        }
    }
}