using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class ToWashingCommand : IChatCommand
    {
        private readonly DialogManager dialogManager;
        public string Command => "Washing";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Washing;

        public ToWashingCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task HandleText(string text, long chatId)
        {
            await dialogManager.ChangeState(DestinationState, chatId, "Стирка", Keyboard.Washing);
        }
    }
}
