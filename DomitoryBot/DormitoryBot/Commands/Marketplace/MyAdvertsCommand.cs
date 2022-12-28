using System.Text;
using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands.Marketplace
{
    public class MyAdvertsCommand : IExecutableCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public MyAdvertsCommand(Lazy<TelegramDialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "MyAdverts";

        public DialogState SourceState => DialogState.Marketplace;

        public DialogState DestinationState => DialogState.Marketplace;

        public async Task Execute(long chatId)
        {
            var adverts = dialogManager.Value.MarketPlace.GetUserAdverts(chatId);

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