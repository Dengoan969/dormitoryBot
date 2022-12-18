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

        public BackCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task HandleText(string text, long chatId)
        {
            //ЗАГОТОВКА КОМАНДЫ
            await dialogManager.StateMenu(chatId);
        }
    }
}
