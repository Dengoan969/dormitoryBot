using System.Text;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands.SubscriptionsService
{
    public class CreateAnnouncementCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public CreateAnnouncementCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "CreateAnnouncement";

        public DialogState SourceState => DialogState.Subscriptions_Manage;

        public DialogState DestinationState => DialogState.Subscriptions_Manage_Announcement_Subscription;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                  "В какую рассылку опубликовать?", Keyboard.Back);
        }
    }
}