using DormitoryBot.Domain.Marketplace;
using DormitoryBot.Domain.Schedule;
using DormitoryBot.Domain.SubscribitionService;
using DormitoryBot.App;
using Ninject;
using Ninject.Extensions.Conventions;
using Telegram.Bot;
using DomitoryBot.App.Commands.Interfaces;
using DomitoryBot.App.Interfaces;
using DomitoryBot.Domain.Marketplace;
using DomitoryBot.Domain.Schedule;
using DomitoryBot.Domain.SubscribitionService;

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
            container.Bind<IUsersStateRepository>().To<MockStateRepository>();
            container.Bind<ISubscriptionRepository>().To<MockSubscriptionRepository>();
            container.Bind<IAdvertsRepository>().To<MockAdvertsRepository>();

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