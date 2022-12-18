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

        public DeleteEntryCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task HandleText(string text, long chatId)
        {
            await dialogManager.StateFAQ(chatId);
        }
    }
}
