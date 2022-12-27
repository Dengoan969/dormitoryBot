using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;
using System.Text;
using Telegram.Bot;

namespace DomitoryBot.Commands.Marketplace
{
    public class MyAdvertsCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public MyAdvertsCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "MyAdverts";

        public DialogState SourceState => DialogState.Marketplace;

        public DialogState DestinationState => DialogState.Marketplace;

        public async Task Execute(long chatId)
        {
            var adverts = dialogManager.Value.MarketPlace.GetUserAdverts(chatId);

            foreach (var advert in adverts)
            {
                var sb = new StringBuilder();
                sb.Append($"{advert.Text}\n");
                sb.Append($"{advert.Price}\n");
                await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, sb.ToString());
            }

            await dialogManager.Value.ChangeState(DestinationState, chatId, "Маркетплейс", Keyboard.Marketplace);
        }
    }
}