using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;
using DormitoryBot.Domain.SubscriptionService;
using Telegram.Bot.Types;

namespace DormitoryBot.App.Commands.SubscriptionsService;

public class HandleDeleteSubscriptionCommand : IHandleTextCommand
{
    private readonly Lazy<IMessageSender> dialogManager;
    private readonly SubscriptionService service;

    public HandleDeleteSubscriptionCommand(Lazy<IMessageSender> dialogManager, SubscriptionService service)
    {
        this.dialogManager = dialogManager;
        this.service = service;
    }

    public DialogState SourceState => DialogState.SubscriptionsManageDeleteSubscription;
    public DialogState DestinationState => DialogState.SubscriptionsManage;


    public async Task HandleMessage(ChatMessage message, long chatId)
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