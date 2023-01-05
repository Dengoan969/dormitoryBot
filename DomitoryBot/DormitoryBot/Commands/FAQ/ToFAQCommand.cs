using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands.FAQ
{
    public class ToFaqCommand : IExecutableCommand
    {
        private readonly Lazy<IDialogSender> dialogManager;

        public ToFaqCommand(Lazy<IDialogSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "FAQ";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Menu;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageAsync(chatId, "Тут умное FAQ");
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId, "Меню",
                DestinationState);
        }
    }
}