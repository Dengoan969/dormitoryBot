using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class ToFreeSlotsCommand : IChatCommand
    {
        private readonly DialogManager dialogManager;
        public string Command => "FreeSlots";

        public DialogState SourceState => DialogState.Washing;

        public DialogState DestinationState => DialogState.Washing;

        public ToFreeSlotsCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task Execute(string text, long chatId)
        {
            await dialogManager.BotClient.SendTextMessageAsync(chatId, "Тут свободное время для начала стирки в виде списка");
            await dialogManager.ChangeState(DestinationState, chatId, "Стирка", Keyboard.Washing);
        }
    }
}
