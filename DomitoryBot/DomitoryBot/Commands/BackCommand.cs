using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class BackCommand : IChatCommand
    {
        private readonly DialogManager dialogManager;
        public string Command => "Back";

        public DialogState SourceState => throw new NotImplementedException();

        public DialogState DestinationState => throw new NotImplementedException();

        public BackCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task Execute(string text, long chatId)
        {
            //ЗАГОТОВКА КОМАНДЫ
            await dialogManager.ChangeState(DestinationState, chatId, "Меню", Keyboard.Menu);
        }
    }
}
