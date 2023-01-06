using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;
using Telegram.Bot.Types;

namespace DormitoryBot.App.Commands.Ideas
{
    public class HandleIdeaTextCommand : IHandleTextCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public HandleIdeaTextCommand(Lazy<IMessageSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public DialogState SourceState => DialogState.IdeaText;

        public DialogState DestinationState => DialogState.Menu;

        public async Task HandleMessage(Message message, long chatId)
        {
            if (message.Text != null)
            {
                Console.WriteLine(message.Text);
                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                    "Меню", DestinationState);
            }
            else
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                    "Какие есть предложения? :)", SourceState);
            }
        }
    }
}