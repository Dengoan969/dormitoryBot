using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands.SubscriptionsService;

public class HandleUnsubscribeCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleUnsubscribeCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Subscriptions_Unsubscribe;
    public DialogState DestinationState => DialogState.Subscriptions;


    public async Task HandleMessage(Message message, long chatId)
    {
        var subscriptionService = dialogManager.Value.SubscriptionService;
        if (message.Text != null && subscriptionService.UnsubscribeUser(chatId, message.Text))
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                                              "Ты успешно отписался :(", Keyboard.Subscriptions);
        }
        else
        {
            await dialogManager.Value.ChangeState(SourceState, chatId,
                                              "Кажется такой подписки у тебя нет, попробуй ещё раз", Keyboard.Back);
        }
    }
}