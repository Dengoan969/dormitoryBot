using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class FreeSlotsCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;
        public string Name => "FreeSlots";

        public DialogState SourceState => DialogState.Washing;

        public DialogState DestinationState => DialogState.Washing;

        public FreeSlotsCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, "Тут свободное время для начала стирки в виде списка");
            await dialogManager.Value.ChangeState(DestinationState, chatId, "Стирка", Keyboard.Washing);
        }
    }
}
