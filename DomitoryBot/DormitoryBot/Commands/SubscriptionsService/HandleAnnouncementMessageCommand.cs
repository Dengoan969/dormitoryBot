using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DormitoryBot.Commands.SubscriptionsService;

public class HandleAnnouncementMessageCommand : IHandleTextCommand
{
    private readonly Lazy<TelegramDialogManager> dialogManager;

    public HandleAnnouncementMessageCommand(Lazy<TelegramDialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.SubscriptionsManageAnnouncementMessage;
    public DialogState DestinationState => DialogState.SubscriptionsManage;


    public async Task HandleMessage(Message message, long chatId)
    {
        var sub = (string) dialogManager.Value.TempInput[chatId][0];
        sub = sub.StartsWith("#") ? sub : "#" + sub;
        var followers = dialogManager.Value.SubscriptionService.GetFollowers(sub);
        switch (message.Type)
        {
            case MessageType.Photo:
            {
                foreach (var user in followers)
                    await dialogManager.Value.SendPhotoAsync(user, message.Photo[0].FileId,
                        $"{sub}\n{message.Caption}");
                break;
            }

            case MessageType.Text:
                foreach (var user in followers)
                    await dialogManager.Value.SendTextMessageAsync(user, $"{sub}\n{message.Text}");
                break;
        }

        await dialogManager.Value.SendTextMessageAsync(chatId, "Круто, всем разослал!");
        await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
            "Управление рассылками", DestinationState, Keyboard.SubscriptionsManage);
    }
}