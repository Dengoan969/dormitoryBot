using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.SubscriptionsService;

public class HandleAnnouncementMessageCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleAnnouncementMessageCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Subscriptions_Manage_Announcement_Message;
    public DialogState DestinationState => DialogState.Subscriptions_Manage;


    public async Task HandleMessage(Message message, long chatId)
    {
        dialogManager.Value.SubscriptionService.SendAnnouncement(dialogManager.Value.BotClient, message,
            (string) dialogManager.Value.temp_input[chatId][0]);
        await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, "Круто, всем разослал!");
        await dialogManager.Value.ChangeState(DestinationState, chatId,
            "Управление рассылками", Keyboard.SubscriptionsManage);
    }
}