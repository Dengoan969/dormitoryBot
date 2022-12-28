using System.Text;
using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands.Marketplace
{
    public class AllAdvertsCommand : IExecutableCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public AllAdvertsCommand(Lazy<TelegramDialogManager> dialogManager)
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
                await dialogManager.Value.SendTextMessageAsync(chatId,
                    "Пока никто не разместил объявлений..");
            }
            else
            {
                await dialogManager.Value.SendTextMessageAsync(chatId, "Все объявления:");
                foreach (var advert in adverts)
                {
                    var sb = new StringBuilder();
                    sb.Append($"{advert.Text}\n\n");
                    sb.Append($"Цена вопроса: {advert.Price}\n");
                    sb.Append($"Писать : @{advert.Username}");
                    await dialogManager.Value.SendTextMessageAsync(chatId, sb.ToString());
                }
            }

            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId, "Маркетплейс",
                DestinationState);
        }
    }
}