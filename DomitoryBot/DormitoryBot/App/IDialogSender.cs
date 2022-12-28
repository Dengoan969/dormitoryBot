namespace DormitoryBot.App;

public interface IDialogSender
{
    Task SendTextMessageAsync(long chatId, string message);
    Task SendPhotoAsync(long chatId, string photoId, string caption);
    Task SendTextMessageWithChangingStateAsync(long chatId, string message, DialogState newState);
}