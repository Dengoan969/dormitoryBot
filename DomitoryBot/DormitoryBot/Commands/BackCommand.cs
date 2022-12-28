﻿using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands
{
    public class BackCommand : IExecutableCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public BackCommand(Lazy<TelegramDialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Back";

        public DialogState SourceState => DialogState.None;

        public DialogState DestinationState => DialogState.Menu;

        public async Task Execute(long chatId)
        {
            //ЗАГОТОВКА КОМАНДЫ
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Меню", DestinationState);
        }
    }
}