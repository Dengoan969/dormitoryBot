using System.Globalization;
using System.Text;
using DomitoryBot.Infrastructure;
using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.Domain.Schedule;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.WashingSchedule;

public class FreeSlotsCommand : IHandleTextCommand
{
    private readonly Lazy<TelegramDialogManager> dialogManager;
    private readonly Schedule schedule;

    public FreeSlotsCommand(Lazy<TelegramDialogManager> dialogManager, Schedule schedule)
    {
        this.dialogManager = dialogManager;
        this.schedule = schedule;
    }

    public DialogState SourceState => DialogState.WashingFreeSlotsChooseDays;
    public DialogState DestinationState => DialogState.Washing;

    public async Task HandleMessage(Message message, long chatId)
    {
        if (DateTime.TryParseExact(message.Text, "d.M", new CultureInfo("ru-RU"), DateTimeStyles.None,
                out var value))
        {
            var freeTimes = schedule.GetFreeTimes();

            foreach (var rec in freeTimes)
            {
                var sb = new StringBuilder();
                sb.Append(rec.Key + "\n");
                foreach (var interval in rec.Value.Where(date => date.Day == value.Day && date.Month == value.Month)
                             .ToList().UnionDateTimeIntervals(30))
                    sb.Append($"{interval.Start.ToString("dd.MM HH:mm")} - {interval.End.ToString("dd.MM HH:mm")}\n");
                if (sb.Length == (rec.Key + "\n").Length)
                    await dialogManager.Value.SendTextMessageAsync(chatId, sb + "Эта дата не доступна");
                else
                    await dialogManager.Value.SendTextMessageAsync(chatId, sb.ToString());
            }

            await dialogManager.Value.SendTextMessageAsync(chatId,
                "Примечание: выбирайте время кратное 30 минутам.\n" +
                "Например в промежутке от 15:00 до 16:00 доступно время 15:00 и 15:30");
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Стирка", DestinationState);
        }

        else
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAsync(chatId,
                "Неправильный ввод", SourceState);
        }
    }
}