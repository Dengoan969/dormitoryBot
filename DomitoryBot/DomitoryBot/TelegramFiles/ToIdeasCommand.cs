﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Telegram
{
    public class ToIdeasCommand : IChatCommand
    {
        private readonly DialogManager dialogManager;
        public string Command => "Ideas";

        public ToIdeasCommand(DialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public async Task HandleText(string text, long chatId)
        {
            await dialogManager.StateIdeas(chatId);
        }
    }
}
