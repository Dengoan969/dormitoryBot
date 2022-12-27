using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.Domain;
using DomitoryBot.Infrastructure;
using Ninject;
using Ninject.Extensions.Conventions;
using Telegram.Bot;

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
            container.Bind<IRecordsRepository>()
                .To<MockScheduleRepository>()
                .WithConstructorArgument("freeTimes", new Dictionary<string, bool[]>
                {
                    {"1", new bool[48 * 3]},
                    {"2", new bool[48 * 3]},
                    {"3", new bool[48 * 3]}
                })
                .WithConstructorArgument("dataBase", new Dictionary<long, List<ScheduleRecord>>());

            container.Bind<IUsersStateRepository>().To<MockStateRepository>()
                .WithConstructorArgument("db", new Dictionary<long, DialogState>());

            container.Bind<ISubscriptionRepository>().To<MockSubscriptionRepository>()
                .WithConstructorArgument("db", new Dictionary<string, Dictionary<long, UserRights>>());

            container.Bind<IAdvertsRepository>().To<MockAdvertsRepository>()
                .WithConstructorArgument("adverts", new SortedSet<Advert>(new AdvertsComparator()));

            container.Bind<DialogManager>().ToSelf().InSingletonScope();

            container.Bind<TelegramBotClient>().ToConstant(new TelegramBotClient(token)).InSingletonScope();

            container.Bind(c =>
                c.FromThisAssembly().SelectAllClasses().InheritedFrom<IChatCommand>().BindAllInterfaces());
            return container.Get<TelegramBotCore>();
        }
    }
}