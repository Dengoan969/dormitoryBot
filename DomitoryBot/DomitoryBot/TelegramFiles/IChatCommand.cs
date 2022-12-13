using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram
{
    public interface IChatCommand
    {
        string Command { get; }

        Task HandleText(string text, long chatId);
    }
}
