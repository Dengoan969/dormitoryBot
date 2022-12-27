using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands.SubscriptionsService;

public class HandleDeleteSubscriptionCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleDeleteSubscriptionCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Subscriptions_Manage_Delete_Subscription;
    public DialogState DestinationState => DialogState.Subscriptions_Manage;


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