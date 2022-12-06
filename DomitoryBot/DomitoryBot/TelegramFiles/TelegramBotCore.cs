using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram
{
    public class TelegramBotCore
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        public Task StartBot(string token)
        {
            var botClient = new TelegramBotClient(token);

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

        private async Task SendMessage(ITelegramBotClient botClient,
            long chatId, string messageText, string? username = null)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: messageText,
                cancellationToken: cts.Token);

            if (username != null)
            {
                Console.WriteLine($"Sent to {username} message \"{messageText}\"");
            }
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Message is not { } message)
                return;
            // Only process text messages
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;
            var username = update.Message.From.Username;

            Console.WriteLine($"Received a '{messageText}' message in chat {username}.");

            await SendMessage(botClient, chatId, "You said: \n" + messageText, username);


            // Echo received message text
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