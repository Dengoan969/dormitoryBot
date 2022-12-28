using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.Marketplace
{
    public class HandleDeleteAdvertCommand : IHandleTextCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public HandleDeleteAdvertCommand(Lazy<TelegramDialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public DialogState SourceState => DialogState.MarketplaceDeleteAdvert;
        public DialogState DestinationState => DialogState.Marketplace;


        public async Task HandleMessage(Message message, long chatId)
        {
            if (message.Text != null)
            {
                var adverts = dialogManager.Value.MarketPlace.GetUserAdverts(chatId);
                if (int.TryParse(message.Text, out var num))
                {
                    if (num > 0 && num <= adverts.Length)
                    {
                        dialogManager.Value.MarketPlace.RemoveAdvert(adverts[num - 1]);
                        await dialogManager.Value.SendTextMessageAsync(chatId, "Объявление удалено!");
                        await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                            "Маркетплейс", DestinationState);
                    }
                    else
                    {
                        await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                            "Кажется это неправильный номер", SourceState);
                    }
                }
                else
                {
                    await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                        "Кажется это не номер..", SourceState);
                }
            }
            else
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                    "Кажется это не номер..", SourceState);
            }
        }
    }
}