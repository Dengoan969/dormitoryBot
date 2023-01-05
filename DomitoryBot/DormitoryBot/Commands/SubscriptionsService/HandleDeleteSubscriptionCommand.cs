using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.Domain.SubscribitionService;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.SubscriptionsService;

public class HandleDeleteSubscriptionCommand : IHandleTextCommand
{
    private readonly Lazy<IDialogSender> dialogManager;
    private readonly SubscriptionService service;

    public HandleDeleteSubscriptionCommand(Lazy<IDialogSender> dialogManager, SubscriptionService service)
    {
        this.dialogManager = dialogManager;
        this.service = service;
    }

    public DialogState SourceState => DialogState.SubscriptionsManageDeleteSubscription;
    public DialogState DestinationState => DialogState.SubscriptionsManage;


    public async Task HandleMessage(Message message, long chatId)
    {
        if (message.Text != null && service.TryDeleteSubscription(message.Text, chatId))
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Рассылка удалена!", DestinationState);
        }
        else
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Кажется ты не админ этой рассылки :(", SourceState);
        }
    }
}