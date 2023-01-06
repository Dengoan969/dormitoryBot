using Telegram.Bot.Types;

namespace DormitoryBot.App.Commands.Interfaces
{
    public interface IHandleTextCommand : IChatCommand
    {
        Task HandleMessage(Message message, long chatId);
    }
}