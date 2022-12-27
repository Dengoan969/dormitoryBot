using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands.Interfaces
{
    public interface IHandleTextCommand : IChatCommand
    {
        Task HandleMessage(Message message, long chatId);
    }
}
