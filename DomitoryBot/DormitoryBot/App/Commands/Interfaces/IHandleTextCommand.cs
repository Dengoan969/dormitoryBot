using Telegram.Bot.Types;

namespace DomitoryBot.App.Commands.Interfaces
{
    public interface IHandleTextCommand : IChatCommand
    {
        Task HandleMessage(Message message, long chatId);
    }
}