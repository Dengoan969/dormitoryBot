using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.Domain.SubscribitionService;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DormitoryBot.Commands.SubscriptionsService;

public class HandleAnnouncementMessageCommand : IHandleTextCommand
{
    private readonly Lazy<IDialogSender> dialogManager;
    private readonly SubscriptionService service;

    public HandleAnnouncementMessageCommand(Lazy<IDialogSender> dialogManager, SubscriptionService service)
    {
        this.dialogManager = dialogManager;
        this.service = service;
    }

    public DialogState SourceState => DialogState.SubscriptionsManageAnnouncementMessage;
    public DialogState DestinationState => DialogState.SubscriptionsManage;


    public async Task HandleMessage(Message message, long chatId)
    {
        var sub = (string) dialogManager.Value.TempInput[chatId][0];
        sub = sub.StartsWith("#") ? sub : "#" + sub;
        var followers = service.GetFollowers(sub);
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
        await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
            "Управление рассылками", DestinationState);
    }
}