using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.App;

namespace DormitoryBot.App.Commands.SubscriptionsService
{
    public class ToSubscriptionsManageCommand : IExecutableCommand
    {
        private readonly Lazy<IMessageSender> dialogManager;

        public ToSubscriptionsManageCommand(Lazy<IMessageSender> dialogManager)
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