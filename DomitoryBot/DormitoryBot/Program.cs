using DormitoryBot.App;
using DormitoryBot.Commands.Interfaces;
using DormitoryBot.Domain;
using DormitoryBot.Infrastructure;
using Ninject;
using Ninject.Extensions.Conventions;
using Telegram.Bot;

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
            container.Bind<IRecordsRepository>()
                .To<MockScheduleRepository>()
                .WithConstructorArgument("freeTimes", new Dictionary<string, bool[]>
                {
                    {"1", new bool[48 * 3]},
                    {"2", new bool[48 * 3]},
                    {"3", new bool[48 * 3]}
                })
                .WithConstructorArgument("dataBase", new Dictionary<long, List<ScheduleRecord>>());

            container.Bind<Schedule>().ToSelf().WithConstructorArgument("washingTypes",
                new Dictionary<string, TimeSpan>
                {
                    {"Полчаса", TimeSpan.FromMinutes(30)},
                    {"Полтора часа", TimeSpan.FromMinutes(90)},
                    {"Два с половиной часа", TimeSpan.FromMinutes(150)}
                }
            );

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