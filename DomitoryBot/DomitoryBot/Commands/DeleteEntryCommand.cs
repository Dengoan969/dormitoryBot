using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class DeleteEntryCommand : IChatCommand
    {
        private readonly DialogManager dialogManager;
        public string Command => "DeleteEntry";

        public DialogState SourceState => DialogState.Washing;

        public DialogState DestinationState => throw new NotImplementedException();

        public DeleteEntryCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task HandleText(string text, long chatId)
        {
            await dialogManager.ChangeState(DestinationState, chatId, "Стирка", Keyboard.Washing);
        }
    }
}
