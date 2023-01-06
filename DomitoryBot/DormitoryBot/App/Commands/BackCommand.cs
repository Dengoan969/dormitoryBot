using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DormitoryBot.App.Commands
{
    public class BackCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public BackCommand(Lazy<IMessageSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Back";

        public DialogState SourceState => DialogState.None;

        public DialogState DestinationState => DialogState.Menu;

        public async Task Execute(long chatId)
        {
            //ЗАГОТОВКА КОМАНДЫ
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Меню", DestinationState);
        }
    }
}