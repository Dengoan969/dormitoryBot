using System.Text;
using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot;

namespace DormitoryBot.Commands.Marketplace
{
    public class AllAdvertsCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public AllAdvertsCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "AllAdverts";

        public DialogState SourceState => DialogState.Marketplace;

        public DialogState DestinationState => DialogState.Marketplace;

        public async Task Execute(long chatId)
        {
            var adverts = dialogManager.Value.MarketPlace.GetAdverts();
            if (adverts.Length == 0)
            {
                await dialogManager.Value.BotClient.SendTextMessageAsync(chatId,
                    "Пока никто не разместил объявлений..");
            }
            else
            {
                await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, "Все объявления:");
                foreach (var advert in adverts)
                {
                    var sb = new StringBuilder();
                    sb.Append($"{advert.Text}\n\n");
                    sb.Append($"Цена вопроса: {advert.Price}\n");
                    sb.Append($"Писать : @{advert.Username}");
                    await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, sb.ToString());
                }
            }

            await dialogManager.Value.ChangeState(DestinationState, chatId, "Маркетплейс", Keyboard.Marketplace);
        }
    }
}