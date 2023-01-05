using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.Domain.SubscribitionService;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.SubscriptionsService;

public class HandleUnsubscribeCommand : IHandleTextCommand
{
    private readonly Lazy<IDialogSender> dialogManager;
    private readonly SubscriptionService service;

    public HandleUnsubscribeCommand(Lazy<IDialogSender> dialogManager, SubscriptionService service)
    {
        this.dialogManager = dialogManager;
        this.service = service;
    }

    public DialogState SourceState => DialogState.SubscriptionsUnsubscribe;
    public DialogState DestinationState => DialogState.Subscriptions;


    public async Task HandleMessage(Message message, long chatId)
    {
        if (message.Text != null && service.UnsubscribeUser(chatId, message.Text))
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