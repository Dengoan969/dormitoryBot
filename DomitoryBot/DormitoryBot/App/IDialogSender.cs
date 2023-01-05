﻿namespace DormitoryBot.App;

public interface IDialogSender
{
    Dictionary<long, List<object>> TempInput { get; }
    Task SendTextMessageAsync(long chatId, string message);
    Task SendPhotoAsync(long chatId, string photoId, string caption);
    Task SendTextMessageWithChangingStateAsync(long chatId, string message, DialogState newState);
}