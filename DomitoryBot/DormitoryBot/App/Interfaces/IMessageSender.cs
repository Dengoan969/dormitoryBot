using DormitoryBot.App;

namespace DormitoryBot.App.Interfaces;

public interface IMessageSender
{
    Dictionary<long, List<object>> TempInput { get; }
    Task SendTextMessageAsync(long chatId, string message);
    Task SendPhotoAsync(long chatId, string photoId, string caption);
    Task SendTextMessageWithChangingStateAsync(long chatId, string message, DialogState newState);
}