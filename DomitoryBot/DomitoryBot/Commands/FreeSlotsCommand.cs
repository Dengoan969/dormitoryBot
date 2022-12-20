using System.Text;
using Telegram;
using Telegram.Bot;

namespace DomitoryBot.Commands
{
    public class FreeSlotsCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public FreeSlotsCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "FreeSlots";

        public DialogState SourceState => DialogState.Washing;

        public DialogState DestinationState => DialogState.Washing;

        public async Task Execute(long chatId)
        {
            var freeTimes = dialogManager.Value.Schedule.GetFreeTimes();

            foreach (var rec in freeTimes)
            {
                var sb = new StringBuilder();
                sb.Append(rec.Key + "\n");
                foreach (var date in rec.Value)
                    sb.Append(date.ToString("dd.MM HH:mm") + "\n");

                sb.Append("\n");
                await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, sb.ToString());
            }

            await dialogManager.Value.ChangeState(DestinationState, chatId, "Стирка", Keyboard.Washing);
        }
    }
}