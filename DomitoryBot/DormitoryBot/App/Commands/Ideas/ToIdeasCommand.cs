using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DomitoryBot.App.Commands.Ideas
{
    public class ToIdeasCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public ToIdeasCommand(Lazy<IMessageSender> dialogManager)
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