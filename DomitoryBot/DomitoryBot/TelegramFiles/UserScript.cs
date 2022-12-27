using DomitoryBot.Commands.Interfaces;
using DomitoryBot.Domain;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram
{
    public class DialogManager
    {
        public readonly TelegramBotClient BotClient;
        public readonly MarketPlace MarketPlace;
        public readonly Schedule Schedule;
        public readonly Dictionary<DialogState, IChatCommand[]> stateCommands;
        public readonly SubscriptionService SubscriptionService;
        public readonly Dictionary<long, List<object>> temp_input = new();
        public readonly IUsersStateRepository USR = new MockStateRepository();

        public DialogManager(TelegramBotClient botClient, IChatCommand[] commands, Schedule schedule,
            MarketPlace marketPlace, SubscriptionService subscriptionService)
        {
            //todo сделать промежуточную сущность, вынести зависимость от телеграма
            BotClient = botClient;
            Schedule = schedule;
            MarketPlace = marketPlace;
            SubscriptionService = subscriptionService;
            stateCommands = new Dictionary<DialogState, IChatCommand[]>();
            foreach (var state in Enum.GetValues<DialogState>())
            {
                stateCommands[state] = commands
                    .Where(x => x.SourceState == state || x.SourceState == DialogState.None
                        && state != DialogState.Start
                        && state != DialogState.Menu)
                    .ToArray();
            }
        }

        public async Task HandleUpdate(Update update)
        {
            if (update.Message?.Date >= DateTime.Now - TimeSpan.FromSeconds(10))
            {
                return;
            }

            var chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message.Chat.Id;
            if (!chatId.HasValue)
            {
                return;
            }

            if (!USR.ContainsKey(chatId.Value))
            {
                await ChangeState(DialogState.Menu, chatId.Value, "Меню", Keyboard.Menu);
            }
            else
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        var input = update.Message;
                        await TryHandleMessage(input, chatId.Value);
                        //await BotClient.DeleteMessageAsync(chatId, update.Message.MessageId);
                        break;
                    case UpdateType.CallbackQuery:
                        var command = update.CallbackQuery.Data;
                        await TryExecuteCommand(command, chatId.Value);
                        await BotClient.DeleteMessageAsync(chatId, update.CallbackQuery.Message.MessageId);
                        break;
                }
            }
        }

        private async Task TryExecuteCommand(string commandName, long chatId)
        {
            var command = (IExecutableCommand) stateCommands[USR.GetState(chatId)]
                .FirstOrDefault(x => x is IExecutableCommand command && command.Name == commandName);
            if(command != null)
            {
                await command.Execute(chatId);
            }
        }

        private async Task TryHandleMessage(Message message, long chatId)
        {
            var command = (IHandleTextCommand) stateCommands[USR.GetState(chatId)].FirstOrDefault(x => x is IHandleTextCommand);
            if (command != null)
            {
                await command.HandleMessage(message, chatId);
            }
            else
            {
                await BotClient.SendTextMessageAsync(chatId, "Прости, но я тебя не понял :(");
            }
        }

        public async Task ChangeState(DialogState newState, long chatId, string message, IReplyMarkup keyboard)
        {
            USR.SetState(chatId, newState);
            await BotClient.SendTextMessageAsync(chatId, message, replyMarkup: keyboard);
        }
    }
}