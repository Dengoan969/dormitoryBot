using System.Globalization;
using System.Text;
using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using Telegram.Bot.Types;

namespace DormitoryBot.Commands.WashingSchedule;

public class FreeSlotsCommand : IHandleTextCommand
{
    private readonly Lazy<TelegramDialogManager> dialogManager;

    public FreeSlotsCommand(Lazy<TelegramDialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.WashingFreeSlotsChooseDays;
    public DialogState DestinationState => DialogState.Washing;

    public async Task HandleMessage(Message message, long chatId)
    {
        if (DateTime.TryParseExact(message.Text, "d.M", new CultureInfo("ru-RU"), DateTimeStyles.None,
                out var value))
        {
            var freeTimes = dialogManager.Value.Schedule.GetFreeTimes();

            foreach (var rec in freeTimes)
            {
                var sb = new StringBuilder();
                sb.Append(rec.Key + "\n");
                var begin = DateTime.MinValue;
                var last = DateTime.MinValue;
                foreach (var date in rec.Value)
                    if (date.Day == value.Day && date.Month == value.Month)
                    {
                        if (begin == DateTime.MinValue && last == DateTime.MinValue)
                        {
                            begin = date;
                            last = date;
                            continue;
                        }

                        if (date - last == TimeSpan.FromMinutes(30))
                        {
                            last = date;
                        }
                        else
                        {
                            sb.Append(
                                $"{begin.ToString("dd.MM HH:mm")} - {last.AddMinutes(30).ToString("dd.MM HH:mm")}\n");
                            begin = date;
                            last = date;
                        }
                    }

                if (!(begin == DateTime.MinValue && last == DateTime.MinValue))
                    sb.Append($"{begin.ToString("dd.MM HH:mm")} - {last.AddMinutes(30).ToString("dd.MM HH:mm")}\n");
                if (sb.Length == (rec.Key + "\n").Length)
                    await dialogManager.Value.SendTextMessageAsync(chatId, sb + "Эта дата не доступна");
                else
                    await dialogManager.Value.SendTextMessageAsync(chatId, sb.ToString());
            }

            await dialogManager.Value.SendTextMessageAsync(chatId,
                "Примечание: выбирайте время кратное 30 минутам.\n" +
                "Например в промежутке от 15:00 до 16:00 доступно время 15:00 и 15:30");
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Стирка", DestinationState);
        }

        else
        {
            await dialogManager.Value.SendTextMessageWithChangingStateAndKeyboardAsync(chatId,
                "Неправильный ввод", SourceState);
        }
    }
}