using System.Text;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class CreateAdvertCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public CreateAdvertCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "CreateAdvert";

        public DialogState SourceState => DialogState.Marketplace;

        public DialogState DestinationState => DialogState.Marketplace_Text;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                  "Введите описание объявления", Keyboard.Back);
        }
    }
}