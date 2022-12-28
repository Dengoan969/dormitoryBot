using System.Text;
using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.Marketplace
{
    public class DeleteAdvertCommand : IExecutableCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public DeleteAdvertCommand(Lazy<TelegramDialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "DeleteAdvert";

        public DialogState SourceState => DialogState.Marketplace;

        public DialogState DestinationState => DialogState.MarketplaceDeleteAdvert;

        public async Task Execute(long chatId)
        {
            var adverts = dialogManager.Value.MarketPlace.GetUserAdverts(chatId);
            if (adverts.Length == 0)
            {
                await dialogManager.Value.SendTextMessageAsync(chatId, "У тебя пока нет объявлений..");
                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId, "Маркетплейс",
                    SourceState, Keyboard.Marketplace);
            }
            else
            {
                await dialogManager.Value.SendTextMessageAsync(chatId, "Твои объявления:");
                for (var i = 0; i < adverts.Length; i++)
                {
                    var advert = adverts[i];
                    var sb = new StringBuilder();
                    sb.Append($"{advert.Text}\n\n");
                    sb.Append($"Цена вопроса: {advert.Price}\n");
                    sb.Append($"Номер объявления: {i + 1}");
                    await dialogManager.Value.SendTextMessageAsync(chatId, sb.ToString());
                }

                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "Введи номер объявления которое хочешь удалить", DestinationState, Keyboard.Back);
            }
        }
    }
}