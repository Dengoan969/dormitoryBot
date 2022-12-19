using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class ToIdeasCommand : IChatCommand
    {
        private readonly DialogManager dialogManager;
        public string Command => "Ideas";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Ideas;

        public ToIdeasCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task HandleText(string text, long chatId)
        {
            await dialogManager.ChangeState(DestinationState, chatId, "Идеи", Keyboard.Ideas);
        }
    }
}
