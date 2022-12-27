using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;

namespace DomitoryBot.Commands
{
    public class ToMenuCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public ToMenuCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "/start";

        public DialogState SourceState => DialogState.Start;

        public DialogState DestinationState => DialogState.Menu;

        public async Task Execute(long chatId)
        {
            dialogManager.Value.temp_input[chatId] = new List<object>();
            await dialogManager.Value.ChangeState(DestinationState, chatId, "Меню", Keyboard.Menu);
        }
    }
}