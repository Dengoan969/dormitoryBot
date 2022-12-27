using System.Text;
using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;
using Telegram.Bot;

namespace DomitoryBot.Commands.WashingSchedule
{
    public class MyEntriesCommand : IExecutableCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public MyEntriesCommand(Lazy<DialogManager> dialogManager)
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
                await dialogManager.Value.ChangeState(DestinationState, chatId, "У вас нет записей", Keyboard.Washing);
            }
            else
            {
                var sb = new StringBuilder();
                sb.Append("Ваши записи:\n");
                for (var i = 0; i < records.Count; i++)
                    sb.Append($"{i + 1}. {records[i].TimeInterval.Start.ToString("dd.MM HH:mm")}" +
                              $" - {records[i].TimeInterval.End.ToString("dd.MM HH:mm")}" +
                              $" Номер машинки: {records[i].Machine}");

                await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, sb.ToString());
                await dialogManager.Value.ChangeState(DestinationState, chatId, "Стирка", Keyboard.Washing);
            }
        }
    }
}