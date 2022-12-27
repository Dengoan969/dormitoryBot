using DomitoryBot.Commands.Interfaces;
using Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands.SubscriptionsService;

public class HandleCreateSubscriptionCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleCreateSubscriptionCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Subscriptions_Manage_Subscription;
    public DialogState DestinationState => DialogState.Subscriptions_Manage;


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