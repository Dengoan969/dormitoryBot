using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands
{
    public class ToMenuCommand : IExecutableCommand
    {
        private readonly Lazy<IDialogSender> dialogManager;

        public ToMenuCommand(Lazy<IDialogSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "/start";

        public DialogState SourceState => DialogState.Start;

        public DialogState DestinationState => DialogState.Menu;

        public async Task Execute(long chatId)
        {
            dialogManager.Value.TempInput[chatId] = new List<object>();
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId, "Меню",
                DestinationState);
        }
    }
}