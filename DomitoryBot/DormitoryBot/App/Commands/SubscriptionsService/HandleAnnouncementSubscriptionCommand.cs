using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;
using DormitoryBot.Domain.SubscriptionService;
using Telegram.Bot.Types;

namespace DormitoryBot.App.Commands.SubscriptionsService;

public class HandleAnnouncementSubscriptionCommand : IHandleTextCommand
{
    private readonly Lazy<IMessageSender> dialogManager;
    private readonly SubscriptionService service;

    public HandleAnnouncementSubscriptionCommand(Lazy<IMessageSender> dialogManager, SubscriptionService service)
    {
        this.dialogManager = dialogManager;
        this.service = service;
    }

    public DialogState SourceState => DialogState.SubscriptionsManageAnnouncementSubscription;
    public DialogState DestinationState => DialogState.SubscriptionsManageAnnouncementMessage;


    public async Task HandleMessage(Message message, long chatId)
    {
        if (message.Text != null && service.IsUserAdmin(chatId, message.Text))
        {
            dialogManager.Value.TempInput[chatId] = new List<object>();
            dialogManager.Value.TempInput[chatId].Add(message.Text);
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Какое сообщение переслать? (можно с фото)", DestinationState);
        }
        else
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Кажется ты не админ этой рассылки :(", SourceState);
        }
    }
}