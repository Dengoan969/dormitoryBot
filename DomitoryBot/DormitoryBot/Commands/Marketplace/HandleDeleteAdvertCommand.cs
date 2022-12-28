using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.Marketplace
{
    public class HandleDeleteAdvertCommand : IHandleTextCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public HandleDeleteAdvertCommand(Lazy<DialogManager> dialogManager)
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
                        await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                            "Маркетплейс", DestinationState, Keyboard.Marketplace);
                    }
                    else
                    {
                        await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                            "Кажется это неправильный номер", SourceState, Keyboard.Back);
                    }
                }
                else
                {
                    await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                        "Кажется это не номер..", SourceState, Keyboard.Back);
                }
            }
            else
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "Кажется это не номер..", SourceState, Keyboard.Back);
            }
        }
    }
}