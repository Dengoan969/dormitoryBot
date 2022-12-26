﻿using System.Globalization;
using System.Text;
using Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands;

public class FreeSlotsCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public FreeSlotsCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Washing_FreeSlots_ChooseDays;
    public DialogState DestinationState => DialogState.Washing;

    public async Task HandleMessage(Message message, long chatId)
    {
        if (DateTime.TryParseExact(message.Text, "dd.MM", new CultureInfo("ru-RU"), DateTimeStyles.None,
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
                await dialogManager.Value.BotClient.SendTextMessageAsync(chatId, sb.ToString());
            }

            await dialogManager.Value.BotClient.SendTextMessageAsync(chatId,
                "Примечание: выбирайте время кратное 30 минутам.\n" +
                "Например в промежутке от 15:00 до 16:00 доступно время 15:00 и 15:30");
            await dialogManager.Value.ChangeState(DestinationState, chatId, "Стирка", Keyboard.Washing);
        }

        else
        {
            await dialogManager.Value.ChangeState(SourceState, chatId, "Неправильный ввод", Keyboard.Back);
        }
    }
}