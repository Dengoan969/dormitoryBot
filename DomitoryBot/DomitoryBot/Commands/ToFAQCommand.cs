using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class ToFAQCommand : IChatCommand
    {
        private readonly DialogManager dialogManager;
        public string Command => "FAQ";

        public ToFAQCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task HandleText(string text, long chatId)
        {
            await dialogManager.StateFAQ(chatId);
        }
    }
}
