using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace DomitoryBot.Commands.Marketplace
{
    public class HandleDeleteAdvertCommand : IHandleTextCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public HandleDeleteAdvertCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public DialogState SourceState => DialogState.Marketplace_Delete_Advert;
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
                        await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, "Объявление удалено!");
                        await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                      "Маркетплейс", Keyboard.Marketplace);
                    }
                    else
                    {
                        await dialogManager.Value.ChangeState(SourceState, chatId,
                                                              "Кажется это неправильный номер", Keyboard.Back);
                    }
                }
                else
                {
                    await dialogManager.Value.ChangeState(SourceState, chatId,
                                                              "Кажется это не номер..", Keyboard.Back);
                }
            }
            else
            {
                await dialogManager.Value.ChangeState(SourceState, chatId,
                                                              "Кажется это не номер..", Keyboard.Back);
            }
        }
    }
}