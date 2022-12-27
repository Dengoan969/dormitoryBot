using Telegram.Bot.Types;

namespace DormitoryBot.Commands.Interfaces
{
    public interface IHandleTextCommand : IChatCommand
    {
        Task HandleMessage(Message message, long chatId);
    }
}