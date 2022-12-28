using System.Text;
using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;

namespace DormitoryBot.Commands.WashingSchedule
{
    public class MyEntriesCommand : IExecutableCommand
    {
        private readonly Lazy<TelegramDialogManager> dialogManager;

        public MyEntriesCommand(Lazy<TelegramDialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        public string Name => "My Records";

        public DialogState SourceState => DialogState.Washing;

        public DialogState DestinationState => DialogState.Washing;

        public async Task Execute(long chatId)
        {
            var records = dialogManager.Value.Schedule.GetRecordsTimesByUser(chatId);

            if (records.Count == 0)
            {
                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                    "У вас нет записей", DestinationState);
            }
            else
            {
                var sb = new StringBuilder();
                sb.Append("Ваши записи:\n");
                for (var i = 0; i < records.Count; i++)
                    sb.Append($"{i + 1}. {records[i].TimeInterval.Start.ToString("dd.MM HH:mm")}" +
                              $" - {records[i].TimeInterval.End.ToString("dd.MM HH:mm")}" +
                              $" Номер машинки: {records[i].Machine}");

                await dialogManager.Value.SendTextMessageAsync(chatId, sb.ToString());
                await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId, "Стирка",
                    DestinationState);
            }
        }
    }
}