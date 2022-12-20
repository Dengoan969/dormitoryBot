using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class MyEntriesCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;
        public string Name => "MyEntries";

        public DialogState SourceState => DialogState.Washing;

        public DialogState DestinationState => DialogState.Washing;

        public MyEntriesCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, "Тут список всех записей");
            await dialogManager.Value.ChangeState(DestinationState, chatId, "Стирка", Keyboard.Washing);
        }
    }
}
