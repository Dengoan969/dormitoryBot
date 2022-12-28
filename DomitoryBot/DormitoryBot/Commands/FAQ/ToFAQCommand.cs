using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot;

namespace DormitoryBot.Commands.FAQ
{
    public class ToFaqCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public ToFaqCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "FAQ";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.Menu;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, "Тут умное FAQ");
            await dialogManager.Value.ChangeState(DestinationState, chatId, "Меню", Keyboard.Menu);
        }
    }
}