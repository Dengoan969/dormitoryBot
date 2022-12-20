﻿using Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands;

public class ToDateSelect : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public ToDateSelect(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Washing_Machine;
    public DialogState DestinationState => DialogState.Washing_Date;

    public async Task HandleMessage(Message message, long chatId)
    {
        if (dialogManager.Value.Schedule.machineNames.Contains(message.Text))
        {
            dialogManager.Value.temp_input[chatId] = new List<object>();
            dialogManager.Value.temp_input[chatId].Add(message.Text);
            await dialogManager.Value.BotClient.SendTextMessageAsync(chatId,
                "Введите дату в формате: число.месяц часы:минуты");
        }
    }
}