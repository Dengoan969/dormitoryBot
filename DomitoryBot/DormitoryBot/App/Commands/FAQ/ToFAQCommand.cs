﻿using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DomitoryBot.App.Commands.FAQ
{
    public class ToFaqCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public ToFaqCommand(Lazy<IMessageSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "FAQ";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Menu;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageAsync(chatId, "Тут умное FAQ");
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId, "Меню",
                DestinationState);
        }
    }
}