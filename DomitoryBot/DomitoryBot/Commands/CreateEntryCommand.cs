using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class CreateEntryCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;
        public string Name => "CreateEntry";

        public DialogState SourceState => DialogState.Washing;

        public DialogState DestinationState => throw new NotImplementedException();

        public CreateEntryCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId, "Стирка", Keyboard.Washing);
        }
    }
}
