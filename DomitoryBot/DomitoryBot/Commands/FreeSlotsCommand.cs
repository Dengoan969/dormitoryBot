using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class FreeSlotsCommand : IChatCommand
    {
        private readonly DialogManager dialogManager;
        public string Command => "FreeSlots";

        public FreeSlotsCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task HandleText(string text, long chatId)
        {
            await dialogManager.BotClient.SendTextMessageAsync(chatId, "Тут свободное время для начала стирки в виде списка");
            await dialogManager.StateWashing(chatId);
        }
    }
}
