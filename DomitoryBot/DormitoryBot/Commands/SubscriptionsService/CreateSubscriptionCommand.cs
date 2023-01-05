﻿using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands.SubscriptionsService
{
    public class CreateSubscriptionCommand : IExecutableCommand
    {
        private readonly Lazy<IDialogSender> dialogManager;

        public CreateSubscriptionCommand(Lazy<IDialogSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "CreateSubscription";

        public DialogState SourceState => DialogState.SubscriptionsManage;

        public DialogState DestinationState => DialogState.SubscriptionsManageCreateSubscription;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Как будет называться рассылка? (Для удобства лучше начинать название с #)", DestinationState);
        }
    }
}