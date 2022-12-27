using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands.SubscriptionsService;

public class HandleSubscribeCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleSubscribeCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Subscriptions_Subscribe;
    public DialogState DestinationState => DialogState.Subscriptions;


    public async Task HandleMessage(Message message, long chatId)
    {
        var subscriptionService = dialogManager.Value.SubscriptionService;
        if (message.Text != null && subscriptionService.SubscribeUser(chatId, message.Text))
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                  "Круто, ты подписался!", Keyboard.Subscriptions);
        }
        else
        {
            await dialogManager.Value.ChangeState(SourceState, chatId,
                                                 "Кажется такой подписки нет или ты уже подписан..", Keyboard.Back);
        }
    }
}