﻿using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.Ideas
{
    public class HandleIdeaTextCommand : IHandleTextCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public HandleIdeaTextCommand(Lazy<TelegramDialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public DialogState SourceState => DialogState.IdeaText;

        public DialogState DestinationState => DialogState.Menu;

        public async Task HandleMessage(Message message, long chatId)
        {
            if (message.Text != null)
            {
                Console.WriteLine(message.Text);
                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "Меню", DestinationState, Keyboard.Menu);
            }
            else
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                    "Какие есть предложения? :)", SourceState, Keyboard.Back);
            }
        }
    }
}