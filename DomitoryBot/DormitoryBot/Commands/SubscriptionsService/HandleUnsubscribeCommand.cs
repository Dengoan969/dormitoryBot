using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.SubscriptionsService;

public class HandleUnsubscribeCommand : IHandleTextCommand
{
    private readonly Lazy<TelegramDialogManager> dialogManager;

    public HandleUnsubscribeCommand(Lazy<TelegramDialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.SubscriptionsUnsubscribe;
    public DialogState DestinationState => DialogState.Subscriptions;


    public async Task HandleMessage(Message message, long chatId)
    {
        var subscriptionService = dialogManager.Value.SubscriptionService;
        if (message.Text != null && subscriptionService.UnsubscribeUser(chatId, message.Text))
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Ты успешно отписался :(", DestinationState);
        }
        else
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Кажется такой подписки у тебя нет, попробуй ещё раз", SourceState);
        }
    }
}