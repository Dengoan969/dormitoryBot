using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands
{
    public class BackCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public BackCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Back";

        public DialogState SourceState => DialogState.None;

        public DialogState DestinationState => DialogState.Menu;

        public async Task Execute(long chatId)
        {
            //ЗАГОТОВКА КОМАНДЫ
            await dialogManager.Value.ChangeState(DestinationState, chatId, "Меню", Keyboard.Menu);
        }
    }
}