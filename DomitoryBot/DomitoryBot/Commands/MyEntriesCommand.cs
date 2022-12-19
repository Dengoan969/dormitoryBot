using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class MyEntriesCommand : IChatCommand
    {
        private readonly DialogManager dialogManager;
        public string Command => "MyEntries";

        public DialogState SourceState => DialogState.Washing;

        public DialogState DestinationState => DialogState.Washing;

        public MyEntriesCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task HandleText(string text, long chatId)
        {
            await dialogManager.BotClient.SendTextMessageAsync(chatId, "Тут список всех записей");
            await dialogManager.ChangeState(DestinationState, chatId, "Стирка", Keyboard.Washing);
        }
    }
}
