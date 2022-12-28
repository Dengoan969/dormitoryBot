namespace DormitoryBot.App;

public interface IDialogSender<in TKeyboard, in TChatID, in TPhotoID>
{
    Task SendTextMessageAsync(TChatID chatId, string message);
    Task SendPhotoAsync(TChatID chatId, TPhotoID photoId, string caption);
    Task SendTextMessageWithChangingStateAndKeyboardAsync(TChatID chatId, string message, DialogState newState);
}