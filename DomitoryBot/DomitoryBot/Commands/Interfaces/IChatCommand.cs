using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;

namespace DomitoryBot.Commands.Interfaces
{
    public interface IChatCommand
    {
        DialogState SourceState { get; }
        DialogState DestinationState { get; }
    }
}
