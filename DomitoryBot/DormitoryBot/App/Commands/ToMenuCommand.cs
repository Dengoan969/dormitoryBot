using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DormitoryBot.App.Commands
{
    public class ToMenuCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public ToMenuCommand(Lazy<IMessageSender> dialogManager)
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