using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class ToFAQCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;
        public string Name => "FAQ";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.FAQ;

        public ToFAQCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId, "FAQ", Keyboard.FAQ);
        }
    }
}
