using System.Text;
using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot;

namespace DormitoryBot.Commands.Marketplace
{
    public class DeleteAdvertCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public DeleteAdvertCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "DeleteAdvert";

        public DialogState SourceState => DialogState.Marketplace;

        public DialogState DestinationState => DialogState.Marketplace_Delete_Advert;

        public async Task Execute(long chatId)
        {
            var adverts = dialogManager.Value.MarketPlace.GetUserAdverts(chatId);
            if (adverts.Length == 0)
            {
                await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, "У тебя пока нет объявлений..");
                await dialogManager.Value.ChangeState(SourceState, chatId, "Маркетплейс", Keyboard.Marketplace);
            }
            else
            {
                await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, "Твои объявления:");
                for (var i = 0; i < adverts.Length; i++)
                {
                    var advert = adverts[i];
                    var sb = new StringBuilder();
                    sb.Append($"{advert.Text}\n\n");
                    sb.Append($"Цена вопроса: {advert.Price}\n");
                    sb.Append($"Номер объявления: {i + 1}");
                    await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, sb.ToString());
                }

                await dialogManager.Value.ChangeState(DestinationState, chatId,
                    "Введи номер объявления которое хочешь удалить", Keyboard.Back);
            }
        }
    }
}