using Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands;

public class HandleAnnouncementCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleAnnouncementCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Subscriptions_Manage_Announcement;
    public DialogState DestinationState => DialogState.Subscriptions_Manage;


    public async Task HandleMessage(Message message, long chatId)
    {
        //dialogManager.Value.SubscriptionService.SendAnnouncement(dialogManager.Value.BotClient, message,)
        if(message.Text != null)
        {
            dialogManager.Value.temp_input[chatId].Add(message.Text);
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                  "На сколько дней разместить объявление?", Keyboard.Back);
        }
        else
        {
            await dialogManager.Value.ChangeState(SourceState, chatId,
                                                  "Напиши что предложишь в награду", Keyboard.Back);
        }
    }
}