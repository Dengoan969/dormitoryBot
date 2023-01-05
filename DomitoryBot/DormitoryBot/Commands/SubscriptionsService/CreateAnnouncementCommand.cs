using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands.SubscriptionsService
{
    public class CreateAnnouncementCommand : IExecutableCommand
    {
        private readonly Lazy<IDialogSender> dialogManager;

        public CreateAnnouncementCommand(Lazy<IDialogSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "CreateAnnouncement";

        public DialogState SourceState => DialogState.SubscriptionsManage;

        public DialogState DestinationState => DialogState.SubscriptionsManageAnnouncementSubscription;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "В какую рассылку опубликовать?", DestinationState);
        }
    }
}