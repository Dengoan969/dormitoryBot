using System.Text;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
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

        public DialogState DestinationState => DialogState.Marketplace_Text;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                  "Напиши объявление (можно вместе с фото)", Keyboard.Back);
        }
    }
}