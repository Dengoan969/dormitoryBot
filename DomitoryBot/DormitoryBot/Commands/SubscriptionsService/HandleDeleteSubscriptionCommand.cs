using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.SubscriptionsService;

public class HandleDeleteSubscriptionCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleDeleteSubscriptionCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.SubscriptionsManageDeleteSubscription;
    public DialogState DestinationState => DialogState.SubscriptionsManage;


    public async Task HandleMessage(Message message, long chatId)
    {
        var subscriptionService = dialogManager.Value.SubscriptionService;
        if (message.Text != null && subscriptionService.TryDeleteSubscription(message.Text, chatId))
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                "Рассылка удалена!", Keyboard.SubscriptionsManage);
        }
        else
        {
            await dialogManager.Value.ChangeState(SourceState, chatId,
                "Кажется ты не админ этой рассылки :(", Keyboard.Back);
        }
    }
}