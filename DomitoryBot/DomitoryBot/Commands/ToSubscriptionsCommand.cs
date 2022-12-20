using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class ToSubscriptionsCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;
        public string Name => "Subscriptions";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Subscriptions;

        public ToSubscriptionsCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId, "Подписки", Keyboard.Subscriptions);
        }
    }
}
