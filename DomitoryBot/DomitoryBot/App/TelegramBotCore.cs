﻿using DomitoryBot.Commands.Interfaces;
using DomitoryBot.Infrastructure;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DomitoryBot.App
{
    public class TelegramBotCore
    {
        private readonly CancellationTokenSource cts;
        private readonly DialogManager dialogManager;
        private readonly TelegramBotClient botClient;

        public TelegramBotCore(DialogManager dialogManager, TelegramBotClient botClient, CancellationTokenSource cts)
        {
            this.cts = cts;
            this.dialogManager = dialogManager;
            this.botClient = botClient;
        }

        public Task StartBot(string token)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };
            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );


            Console.ReadLine();
            cts.Cancel();
            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            await dialogManager?.HandleUpdate(update);
        }


        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}