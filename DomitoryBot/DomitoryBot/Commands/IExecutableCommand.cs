using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;

namespace DomitoryBot.Commands
{
    public interface IExecutableCommand : IChatCommand
    {
        public string Name { get; }
        Task Execute(long chatId);
    }
}
