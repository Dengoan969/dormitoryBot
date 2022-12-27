using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands.SubscriptionsService;

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
        dialogManager.Value.SubscriptionService.SendAnnouncement(dialogManager.Value.BotClient, message, (string)dialogManager.Value.temp_input[chatId][0]);
        await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                 "Круто, всем разослал!", Keyboard.SubscriptionsManage);
    }
}