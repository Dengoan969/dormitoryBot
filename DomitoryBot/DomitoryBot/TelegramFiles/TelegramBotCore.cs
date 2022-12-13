using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram
{
    public class TelegramBotCore
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        DialogManager dialogManager = new DialogManager();

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
            switch (update.Type)
            {
                case UpdateType.Message:
                    dialogManager.HandleUpdate(botClient, update);
                    break;
                    //var text = update.Message.Text;
                    //switch (text)
                    //{
                    //    case "Button1":
                    //        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Button1", replyMarkup: GetButtons());
                    //        break;
                    //    case "Button2":
                    //        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Button2", replyMarkup: GetButtons());
                    //        break;
                    //    case "Button3":
                    //        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Button3", replyMarkup: GetButtons());
                    //        break;
                    //    case "Button4":
                    //        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Button4", replyMarkup: GetButtons());
                    //        break;
                    //    default:
                    //        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "You said: \n" + text, replyMarkup:GetButtons());
                    //        break;
                    //}
            }


            //var username = update.Message.From.Username;
            //Console.WriteLine($"Received a '{messageText}' message in chat {username}.");

        }

        private IReplyMarkup GetButtons()
        {
            return Keyboard.Menu;
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