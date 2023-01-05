using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands.Ideas
{
    public class ToIdeasCommand : IExecutableCommand
    {
        private readonly Lazy<IDialogSender> dialogManager;

        public ToIdeasCommand(Lazy<IDialogSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "Ideas";

        public DialogState SourceState => DialogState.Menu;

        public DialogState DestinationState => DialogState.IdeaText;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Какие есть предложения? :)", DestinationState);
        }
    }
}