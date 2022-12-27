using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.Ideas
{
    public class HandleIdeaTextCommand : IHandleTextCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public HandleIdeaTextCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public DialogState SourceState => DialogState.Idea_Text;

        public DialogState DestinationState => DialogState.Menu;

        public async Task HandleMessage(Message message, long chatId)
        {
            if (message.Text != null)
            {
                Console.WriteLine(message.Text);
                await dialogManager.Value.ChangeState(DestinationState, chatId,
                    "Меню", Keyboard.Menu);
            }
            else
            {
                await dialogManager.Value.ChangeState(SourceState, chatId,
                    "Какие есть предложения? :)", Keyboard.Back);
            }
        }
    }
}