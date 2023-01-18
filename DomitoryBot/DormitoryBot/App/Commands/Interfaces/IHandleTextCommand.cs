using Telegram.Bot.Types;

namespace DormitoryBot.App.Commands.Interfaces
{
    public interface IHandleTextCommand : IChatCommand
    {
        Task HandleMessage(ChatMessage message, long chatId);
    }
}