﻿using System.Text;
using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;
using DormitoryBot.Domain.Marketplace;

namespace DormitoryBot.App.Commands.Marketplace
{
    public class AllAdvertsCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;
        private readonly MarketPlace marketPlace;

        public AllAdvertsCommand(Lazy<IMessageSender> dialogManager, MarketPlace marketPlace)
        {
            this.dialogManager = dialogManager;
            this.marketPlace = marketPlace;
        }

        public string Name => "AllAdverts";

        public DialogState SourceState => DialogState.Marketplace;

        public DialogState DestinationState => DialogState.Marketplace;

        public async Task Execute(long chatId)
        {
            var adverts = marketPlace.Adverts;
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