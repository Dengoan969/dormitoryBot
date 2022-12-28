using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.FAQ
{
    public class ToFaqCommand : IExecutableCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public ToFaqCommand(Lazy<TelegramDialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "FAQ";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Menu;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageAsync(chatId, "Тут умное FAQ");
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId, "Меню", DestinationState,
                Keyboard.Menu);
        }
    }
}