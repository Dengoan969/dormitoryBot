using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands.SubscriptionsService
{
    public class ToSubscriptionsManageCommand : IExecutableCommand
    {
        private readonly Lazy<IDialogSender> dialogManager;

        public ToSubscriptionsManageCommand(Lazy<IDialogSender> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "SubscriptionsManage";

        public DialogState SourceState => DialogState.Subscriptions;

        public DialogState DestinationState => DialogState.SubscriptionsManage;

        public async Task Execute(long chatId)
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Управление рассылками", DestinationState);
        }
    }
}