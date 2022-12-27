using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands.SubscriptionsService;

public class HandleAnnouncementSubscriptionCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleAnnouncementSubscriptionCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Subscriptions_Manage_Announcement_Subscription;
    public DialogState DestinationState => DialogState.Subscriptions_Manage_Announcement_Message;


    public async Task HandleMessage(Message message, long chatId)
    {
        var subscriptionService = dialogManager.Value.SubscriptionService;
        if (message.Text != null && subscriptionService.IsUserAdmin(chatId, message.Text))
        {
            dialogManager.Value.temp_input[chatId] = new List<object>();
            dialogManager.Value.temp_input[chatId].Add(message.Text);
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                 "Какое сообщение переслать? (можно с фото)", Keyboard.Back);
        }
        else
        {
            await dialogManager.Value.ChangeState(SourceState, chatId,
                                                 "Кажется ты не админ этой рассылки :(", Keyboard.Back);
        }
    }
}