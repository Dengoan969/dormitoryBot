using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class ToMarketplaceCommand : IChatCommand
    {
        private readonly DialogManager dialogManager;
        public string Command => "Marketplace";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Marketplace;

        public ToMarketplaceCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task HandleText(string text, long chatId)
        {
            await dialogManager.ChangeState(DestinationState, chatId, "Маркетплейс", Keyboard.Marketplace);
        }
    }
}
