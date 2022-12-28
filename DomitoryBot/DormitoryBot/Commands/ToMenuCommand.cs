﻿using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands
{
    public class ToMenuCommand : IExecutableCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public ToMenuCommand(Lazy<TelegramDialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "/start";

        public DialogState SourceState => DialogState.Start;

        public DialogState DestinationState => DialogState.Menu;

        public async Task Execute(long chatId)
        {
            dialogManager.Value.TempInput[chatId] = new List<object>();
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId, "Меню", DestinationState,
                Keyboard.Menu);
        }
    }
}