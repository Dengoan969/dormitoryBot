using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.SubscriptionsService
{
    public class CreateAnnouncementCommand : IExecutableCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public CreateAnnouncementCommand(Lazy<TelegramDialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "CreateAnnouncement";

        public DialogState SourceState => DialogState.SubscriptionsManage;

        public DialogState DestinationState => DialogState.SubscriptionsManageAnnouncementSubscription;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "В какую рассылку опубликовать?", DestinationState, Keyboard.Back);
        }
    }
}