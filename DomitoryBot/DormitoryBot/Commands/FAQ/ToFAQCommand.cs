using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands.FAQ
{
    public class ToFaqCommand : IExecutableCommand
    {
        private readonly Lazy<ITelegramDialogSender> dialogManager;

        public ToFaqCommand(Lazy<ITelegramDialogSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "FAQ";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Menu;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageAsync(chatId, "Тут умное FAQ");
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId, "Меню",
                DestinationState);
        }
    }
}