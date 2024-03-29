﻿using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;
using DormitoryBot.Domain.Marketplace;
using Telegram.Bot.Types;

namespace DormitoryBot.App.Commands.Marketplace
{
    public class HandleDeleteAdvertCommand : IHandleTextCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;
        private readonly MarketPlace marketPlace;

        public HandleDeleteAdvertCommand(Lazy<IMessageSender> dialogManager, MarketPlace marketPlace)
        {
            this.dialogManager = dialogManager;
            this.marketPlace = marketPlace;
        }

        public DialogState SourceState => DialogState.MarketplaceDeleteAdvert;
        public DialogState DestinationState => DialogState.Marketplace;


        public async Task HandleMessage(ChatMessage message, long chatId)
        {
            if (message.Text != null)
            {
                var adverts = marketPlace.GetUserAdverts(chatId);
                if (int.TryParse(message.Text, out var num))
                {
                    if (num > 0 && num <= adverts.Length)
                    {
                        marketPlace.RemoveAdvert(adverts[num - 1]);
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