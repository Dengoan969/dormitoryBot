using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.Domain.SubscribitionService;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.SubscriptionsService;

public class HandleSubscribeCommand : IHandleTextCommand
{
    private readonly Lazy<IDialogSender> dialogManager;
    private readonly SubscriptionService service;

    public HandleSubscribeCommand(Lazy<IDialogSender> dialogManager, SubscriptionService service)
    {
        this.dialogManager = dialogManager;
        this.service = service;
    }

    public DialogState SourceState => DialogState.SubscriptionsSubscribe;
    public DialogState DestinationState => DialogState.Subscriptions;


    public async Task HandleMessage(Message message, long chatId)
    {
        if (message.Text != null && service.TrySubscribeUser(chatId, message.Text))
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Круто, ты подписался!", DestinationState);
        }
        else
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Кажется такой подписки нет или ты уже подписан..", SourceState);
        }
    }
}