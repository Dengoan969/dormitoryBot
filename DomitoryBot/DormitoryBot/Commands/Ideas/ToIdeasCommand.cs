using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.Ideas
{
    public class ToIdeasCommand : IExecutableCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public ToIdeasCommand(Lazy<TelegramDialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Ideas";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.IdeaText;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Какие есть предложения? :)", DestinationState, Keyboard.Back);
        }
    }
}