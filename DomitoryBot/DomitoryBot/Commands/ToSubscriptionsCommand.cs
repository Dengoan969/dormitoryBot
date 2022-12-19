using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class ToSubscriptionsCommand : IChatCommand
    {
        private readonly DialogManager dialogManager;
        public string Command => "Subscriptions";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Subscriptions;

        public ToSubscriptionsCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task HandleText(string text, long chatId)
        {
            await dialogManager.ChangeState(DestinationState, chatId, "Подписки", Keyboard.Subscriptions);
        }
    }
}
