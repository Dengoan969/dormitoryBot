using DormitoryBot.Commands.Interfaces;
using DormitoryBot.Domain.Marketplace;
using DormitoryBot.Domain.Schedule;
using DormitoryBot.Domain.SubscribitionService;
using DormitoryBot.Infrastructure;
using DormitoryBot.UI;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DormitoryBot.App
{
    public class TelegramDialogManager : ITelegramDialogSender, ITelegramUpdateHandler
    {
        private readonly TelegramBotClient BotClient;
        public readonly MarketPlace MarketPlace;
        public readonly Schedule Schedule;
        public readonly Dictionary<DialogState, IChatCommand[]> StateCommands;
        public readonly SubscriptionService SubscriptionService;
        public readonly Dictionary<long, List<object>> TempInput = new();
        public readonly IUsersStateRepository Usr;

        public TelegramDialogManager(TelegramBotClient botClient, IChatCommand[] commands, Schedule schedule,
            MarketPlace marketPlace, SubscriptionService subscriptionService, IUsersStateRepository usr)
        {
            BotClient = botClient;
            Schedule = schedule;
            MarketPlace = marketPlace;
            SubscriptionService = subscriptionService;
            StateCommands = new Dictionary<DialogState, IChatCommand[]>();
            foreach (var state in Enum.GetValues<DialogState>())
            {
                StateCommands[state] = commands
                    .Where(x => x.SourceState == state || x.SourceState == DialogState.None
                        && state != DialogState.Start
                        && state != DialogState.Menu)
                    .ToArray();
            }

            Usr = usr;
        }

        public async Task SendTextMessageAsync(long chatId, string message)
        {
            await BotClient.SendTextMessageAsync(chatId, message);
        }

        public async Task SendPhotoAsync(long chatId, string photoId, string? caption = null)
        {
            await BotClient.SendPhotoAsync(chatId, photoId, caption);
        }

        public async Task SendTextMessageWithChangingStateAsync(long chatId, string message,
            DialogState newState)
        {
            Usr.SetState(chatId, newState);
            var keyboard = Keyboard.GetKeyboardByState(newState);
            await BotClient.SendTextMessageAsync(chatId, message, replyMarkup: keyboard);
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

            if (!Usr.ContainsKey(chatId.Value))
            {
                await SendTextMessageWithChangingStateAsync(chatId.Value, "Меню", DialogState.Menu);
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
            var command = (IExecutableCommand) StateCommands[Usr.GetState(chatId)]
                .FirstOrDefault(x => x is IExecutableCommand command && command.Name == commandName);
            if (command != null)
            {
                await command.Execute(chatId);
            }
        }

        private async Task TryHandleMessage(Message message, long chatId)
        {
            var command =
                (IHandleTextCommand) StateCommands[Usr.GetState(chatId)].FirstOrDefault(x => x is IHandleTextCommand);
            if (command != null)
            {
                await command.HandleMessage(message, chatId);
            }
            else
            {
                await BotClient.SendTextMessageAsync(chatId, "Прости, но я тебя не понял :(");
            }
        }
    }
}