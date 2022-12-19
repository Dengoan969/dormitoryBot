using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class CreateEntryCommand : IChatCommand
    {
        private readonly DialogManager dialogManager;
        public string Command => "CreateEntry";

        public DialogState SourceState => DialogState.Washing;

        public DialogState DestinationState => throw new NotImplementedException();

        public CreateEntryCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task HandleText(string text, long chatId)
        {
            await dialogManager.ChangeState(DestinationState, chatId, "Стирка", Keyboard.Washing);
        }
    }
}
