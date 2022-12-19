using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;

namespace DomitoryBot.Commands
{
    public interface IChatCommand
    {
        string Command { get; }
        DialogState SourceState { get; }
        DialogState DestinationState { get; }
        //source state, dest
        Task HandleText(string text, long chatId);
    }
}
