using DormitoryBot.Domain.Marketplace;
using DormitoryBot.Domain.Schedule;
using DormitoryBot.Domain.SubscriptionService;
using DormitoryBot.App;
using DormitoryBot.Infrastructure;
using Ninject;
using Ninject.Extensions.Conventions;
using Telegram.Bot;
using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using System.Configuration;

namespace DormitoryBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var token = Console.ReadLine();
            var bot = CreateBot(token);
            bot.StartBot(token).GetAwaiter().GetResult();
        }

        private static TelegramBotCore CreateBot(string token)
        {
            var container = new StandardKernel();
            container.Settings.AllowNullInjection = true;

            container.Bind<IRecordsRepository>().To<MockScheduleRepository>();
            container.Bind<IUsersStateRepository>().To<PgSqlUsersStateRepository>().InSingletonScope();
            container.Bind<ISubscriptionRepository>().To<PgSqlSubscriptionRepository>().InSingletonScope();
            container.Bind<IAdvertsRepository>().To<PgSqlAdvertsRepository>().InSingletonScope();

            container.Bind<IDateTimeService>().To<DefaultDateTimeService>().InSingletonScope();

            container.Bind<MarketPlace>().ToSelf().InSingletonScope();
            container.Bind<Schedule>().ToSelf().InSingletonScope();
            container.Bind<SubscriptionService>().ToSelf().InSingletonScope();

            container.Bind(c =>
                c.FromThisAssembly().SelectAllClasses().InheritedFrom<IChatCommand>()
                .BindAllInterfaces());

            container.Bind<ITelegramUpdateHandler, IMessageSender, ITelegramDialogSender>()
                .To<TelegramDialogManager>()
                .InSingletonScope();

            container.Bind<TelegramBotClient>().ToSelf().WithConstructorArgument("token", token);

            return container.Get<TelegramBotCore>();
        }
    }
}