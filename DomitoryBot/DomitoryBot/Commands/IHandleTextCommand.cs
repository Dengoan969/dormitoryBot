using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;

namespace DomitoryBot.Commands
{
    public interface IHandleTextCommand : IChatCommand
    {
        Task HandleText(string text, long chatId);
    }
}
