using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.UI;

namespace DormitoryBot.Commands.SubscriptionsService
{
    public class ToSubscriptionsManageCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public ToSubscriptionsManageCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "SubscriptionsManage";

        public DialogState SourceState => DialogState.Subscriptions;

        public DialogState DestinationState => DialogState.SubscriptionsManage;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Управление рассылками", DestinationState, Keyboard.SubscriptionsManage);
        }
    }
}