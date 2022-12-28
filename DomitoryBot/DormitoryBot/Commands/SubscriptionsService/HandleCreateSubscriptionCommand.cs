using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.SubscriptionsService;

public class HandleCreateSubscriptionCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleCreateSubscriptionCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.SubscriptionsManageCreateSubscription;
    public DialogState DestinationState => DialogState.SubscriptionsManage;


    public async Task HandleMessage(Message message, long chatId)
    {
        var subscriptionService = dialogManager.Value.SubscriptionService;
        if (message.Text != null && subscriptionService.TryCreateSubscription(message.Text, chatId))
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                "Круто, ты создал рассылку!", Keyboard.SubscriptionsManage);
        }
        else
        {
            await dialogManager.Value.ChangeState(SourceState, chatId,
                "Кажется такая рассылка уже есть :( Попробуй другое название", Keyboard.Back);
        }
    }
}