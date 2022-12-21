using DomitoryBot.Commands;
using DomitoryBot.Domain;
using Ninject;
using Ninject.Extensions.Conventions;
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
        DialogManager dialogManager;

        public Task StartBot(string token)
        {
            var botClient = new TelegramBotClient(token);
            var container = new StandardKernel();
            container.Bind<IRecordsRepository>().To<MockScheduleRepository>();
            container.Bind<ISubscriptionRepository>().To<MockSubscriptionRepository>();
            container.Bind<IAdvertsRepository>().To<MockAdvertsRepository>();
            container.Bind<DialogManager>().ToSelf().InSingletonScope();
            container.Bind<TelegramBotClient>().ToConstant(botClient);

            container.Bind(c =>
                c.FromThisAssembly().SelectAllClasses().InheritedFrom<IChatCommand>().BindAllInterfaces());
            dialogManager = container.Get<DialogManager>();

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