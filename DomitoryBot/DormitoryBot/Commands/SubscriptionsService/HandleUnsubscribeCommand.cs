using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.SubscriptionsService;

public class HandleUnsubscribeCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleUnsubscribeCommand(Lazy<DialogManager> dialogManager)
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
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Ты успешно отписался :(", DestinationState, Keyboard.Subscriptions);
        }
        else
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Кажется такой подписки у тебя нет, попробуй ещё раз", SourceState, Keyboard.Back);
        }
    }
}