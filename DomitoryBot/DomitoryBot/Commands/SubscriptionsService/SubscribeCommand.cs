﻿using System.Text;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands.SubscriptionsService
{
    public class SubscribeCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public SubscribeCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Subscribe";

        public DialogState SourceState => DialogState.Subscriptions;

        public DialogState DestinationState => DialogState.Subscriptions_Subscribe;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                  "На какую рассылку подписаться?", Keyboard.Back);
        }
    }
}