using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class ToWashingCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;
        public string Name => "Washing";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Washing;

        public ToWashingCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId, "Стирка", Keyboard.Washing);
        }
    }
}
