using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.SubscriptionsService;

public class HandleAnnouncementSubscriptionCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleAnnouncementSubscriptionCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.SubscriptionsManageAnnouncementSubscription;
    public DialogState DestinationState => DialogState.SubscriptionsManageAnnouncementMessage;


    public async Task HandleMessage(Message message, long chatId)
    {
        var subscriptionService = dialogManager.Value.SubscriptionService;
        if (message.Text != null && subscriptionService.IsUserAdmin(chatId, message.Text))
        {
            dialogManager.Value.TempInput[chatId] = new List<object>();
            dialogManager.Value.TempInput[chatId].Add(message.Text);
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Какое сообщение переслать? (можно с фото)", DestinationState, Keyboard.Back);
        }
        else
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Кажется ты не админ этой рассылки :(", SourceState, Keyboard.Back);
        }
    }
}