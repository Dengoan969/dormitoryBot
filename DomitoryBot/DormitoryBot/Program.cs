using DormitoryBot.Domain.Marketplace;
using DormitoryBot.Domain.Schedule;
using DormitoryBot.Domain.SubscribitionService;
using DormitoryBot.App;
using DormitoryBot.Infrastructure;
using Ninject;
using Ninject.Extensions.Conventions;
using Telegram.Bot;
using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;

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

            container.Bind<IRecordsRepository>().To<MockScheduleRepository>();
            container.Bind<IUsersStateRepository>().To<MockStateRepository>();
            container.Bind<ISubscriptionRepository>().To<MockSubscriptionRepository>();
            container.Bind<IAdvertsRepository>().To<MockAdvertsRepository>();

            container.Bind<ITelegramUpdateHandler, IMessageSender, ITelegramDialogSender>()
                .To<TelegramDialogManager>()
                .InSingletonScope();

            container.Bind<TelegramBotClient>().ToConstant(new TelegramBotClient(token)).InSingletonScope();

            container.Bind(c =>
                c.FromThisAssembly().SelectAllClasses().InheritedFrom<IChatCommand>()
                .BindAllInterfaces());

            return container.Get<TelegramBotCore>();
        }
    }
}