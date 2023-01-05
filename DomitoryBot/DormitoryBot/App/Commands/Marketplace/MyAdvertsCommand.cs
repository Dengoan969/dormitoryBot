using System.Text;
using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;
using DormitoryBot.App;
using DormitoryBot.Domain.Marketplace;

namespace DomitoryBot.App.Commands.Marketplace
{
    public class MyAdvertsCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;
        private readonly MarketPlace marketPlace;

        public MyAdvertsCommand(Lazy<IMessageSender> dialogManager, MarketPlace marketPlace)
        {
            this.dialogManager = dialogManager;
            this.marketPlace = marketPlace;
        }

        public string Name => "MyAdverts";

        public DialogState SourceState => DialogState.Marketplace;

        public DialogState DestinationState => DialogState.Marketplace;

        public async Task Execute(long chatId)
        {
            var adverts = marketPlace.GetUserAdverts(chatId);

            if (adverts.Length == 0)
            {
                await dialogManager.Value.SendTextMessageAsync(chatId, "У тебя пока нет объявлений..");
            }
            else
            {
                await dialogManager.Value.SendTextMessageAsync(chatId, "Твои объявления:");
                foreach (var advert in adverts)
                {
                    var sb = new StringBuilder();
                    sb.Append($"{advert.Text}\n\n");
                    sb.Append($"Цена вопроса: {advert.Price}\n");
                    sb.Append($"Писать: @{advert.Username}");
                    await dialogManager.Value.SendTextMessageAsync(chatId, sb.ToString());
                }
            }

            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Маркетплейс", DestinationState);
        }
    }
}