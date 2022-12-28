namespace DormitoryBot.App;

public interface IDialogManager<in TUpdate, in TKeyboard, in TChatID, in TPhotoID>
{
    Task SendTextMessageAsync(long chatId, string message);
    Task SendPhotoAsync(TChatID chatId, TPhotoID photoId, string caption);

    Task SendTextMessageWithChangingStateAndKeyboardAsync(long chatId, string message, DialogState newState,
        TKeyboard keyboard);

    Task HandleUpdate(TUpdate update);
}