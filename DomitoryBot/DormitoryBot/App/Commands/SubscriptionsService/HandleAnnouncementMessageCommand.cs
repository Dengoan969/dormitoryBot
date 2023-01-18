using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;
using DormitoryBot.Domain.SubscriptionService;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DormitoryBot.App.Commands.SubscriptionsService;

public class HandleAnnouncementMessageCommand : IHandleTextCommand
{
    private readonly Lazy<IMessageSender> dialogManager;
    private readonly SubscriptionService service;

    public HandleAnnouncementMessageCommand(Lazy<IMessageSender> dialogManager, SubscriptionService service)
    {
        this.dialogManager = dialogManager;
        this.service = service;
    }

    public DialogState SourceState => DialogState.SubscriptionsManageAnnouncementMessage;
    public DialogState DestinationState => DialogState.SubscriptionsManage;


    public async Task HandleMessage(ChatMessage message, long chatId)
    {
        var sub = (string)dialogManager.Value.TempInput[chatId][0];
        sub = sub.StartsWith("#") ? sub : "#" + sub;
        var followers = service.GetFollowers(sub);
        if(message.PhotoIds != null)
        {
            foreach (var user in followers)
                await dialogManager.Value.SendPhotoAsync(user, message.PhotoIds[0],
                    $"{sub}\n{message.Caption}");
        }
        else
        {
            foreach (var user in followers)
                await dialogManager.Value.SendTextMessageAsync(user, $"{sub}\n{message.Text}");
        }

        await dialogManager.Value.SendTextMessageAsync(chatId, "Круто, всем разослал!");
        await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
            "Управление рассылками", DestinationState);
    }
}