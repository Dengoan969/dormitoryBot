using DomitoryBot.App;
using DomitoryBot.Infrastructure;
using DomitoryBot.Commands.Interfaces;
using Telegram.Bot;
using Ninject;
using Ninject.Extensions.Conventions;

namespace DomitoryBot
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
            container.Bind<IRecordsRepository>().To<MockScheduleRepository>();
            container.Bind<ISubscriptionRepository>().To<MockSubscriptionRepository>();
            container.Bind<IAdvertsRepository>().To<MockAdvertsRepository>();

            container.Bind<DialogManager>().ToSelf().InSingletonScope();

            container.Bind<TelegramBotClient>().ToConstant(new TelegramBotClient(token)).InSingletonScope();

            container.Bind<string[]>().ToConstant(new[] { "1", "2", "3" }).WhenInjectedExactlyInto<Schedule>();

            container.Bind(c =>
                c.FromThisAssembly().SelectAllClasses().InheritedFrom<IChatCommand>().BindAllInterfaces());
            return container.Get<TelegramBotCore>();
        }
    }
}