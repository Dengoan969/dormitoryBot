using DomitoryBot.Commands;
using DomitoryBot.Domain;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram
{
    //[Flags]
    public enum DialogState
    {
        Start,
        Menu,
        Washing,
        Washing_Date,
        Washing_Machine,
        Marketplace,
        Subscriptions,
        FAQ,
        Ideas,
        None
    }

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
                        await BotClient.DeleteMessageAsync(chatId, update.Message.MessageId);
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
                .First(x => x is IExecutableCommand command && command.Name == commandName);
            await command.Execute(chatId);
        }

        private async Task TryHandleMessage(Message message, long chatId)
        {
            var command = (IHandleTextCommand) stateCommands[USR.GetState(chatId)].First(x => x is IHandleTextCommand);
            await command.HandleMessage(message, chatId);
        }

        public async Task ChangeState(DialogState newState, long chatId, string message, IReplyMarkup keyboard)
        {
            USR.SetState(chatId, newState);
            await BotClient.SendTextMessageAsync(chatId, message, replyMarkup: keyboard);
        }
    }

    public interface IUsersStateRepository
    {
        public DialogState GetState(long id);
        public bool ContainsKey(long id);
        public void SetState(long id, DialogState state);
    }

    public class MockStateRepository : IUsersStateRepository
    {
        private readonly Dictionary<long, DialogState> db = new();


        public DialogState GetState(long id)
        {
            if (!db.ContainsKey(id)) throw new ArgumentException("User doesn't exists");

            return db[id];
        }

        public bool ContainsKey(long id)
        {
            return db.ContainsKey(id);
        }

        public void SetState(long id, DialogState state)
        {
            db[id] = state;
        }
    }

    public static class Keyboard
    {
        public static InlineKeyboardMarkup Menu = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Стирка", "Washing"),
                InlineKeyboardButton.WithCallbackData("Маркетплейс", "Marketplace"),
                InlineKeyboardButton.WithCallbackData("Объявления", "Subscriptions")
            },

            new[]
            {
                InlineKeyboardButton.WithCallbackData("FAQ", "FAQ"),
                InlineKeyboardButton.WithCallbackData("Предложить идею", "Ideas")
            }
        });

        //public static ReplyKeyboardMarkup Back = new ReplyKeyboardMarkup(new KeyboardButton("Назад"));

        public static InlineKeyboardMarkup Washing = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Свободные слоты", "FreeSlots"),
                InlineKeyboardButton.WithCallbackData("Мои записи", "MyEntries"),
                InlineKeyboardButton.WithCallbackData("Назад", "Back")
            },

            new[]
            {
                InlineKeyboardButton.WithCallbackData("Записаться", "CreateEntry"),
                InlineKeyboardButton.WithCallbackData("Удалить запись", "DeleteEntry")
            }
        });

        //public static InlineKeyboardMarkup Washing_Date = new InlineKeyboardMarkup(new[] {
        //    new [] {InlineKeyboardButton.WithCallbackData("Сегодня","FreeSlots"),
        //            InlineKeyboardButton.WithCallbackData("Завтра","MyEntries"),
        //            InlineKeyboardButton.WithCallbackData("Послезавтра","Back"),
        //            InlineKeyboardButton.WithCallbackData("Назад","Back")} });

        public static InlineKeyboardMarkup Washing_Machine = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("1", "1"),
                InlineKeyboardButton.WithCallbackData("2", "2"),
                InlineKeyboardButton.WithCallbackData("3", "3"),
                InlineKeyboardButton.WithCallbackData("Назад", "Back")
            }
        });

        public static InlineKeyboardMarkup Marketplace = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Все объявления", "FreeSlots"),
                InlineKeyboardButton.WithCallbackData("Мои объявления", "MyEntries")
            },

            new[]
            {
                InlineKeyboardButton.WithCallbackData("Создать объявление", "CreateEntry"),
                InlineKeyboardButton.WithCallbackData("Назад", "DeleteEntry")
            }
        });

        public static InlineKeyboardMarkup Back = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Назад", "DeleteEntry")
            }
        });

        public static InlineKeyboardMarkup Subscriptions = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Подписаться", "FreeSlots"),
                InlineKeyboardButton.WithCallbackData("Отписаться", "MyEntries")
            },

            new[]
            {
                InlineKeyboardButton.WithCallbackData("Создать объявление", "CreateEntry"),
                InlineKeyboardButton.WithCallbackData("Назад", "DeleteEntry")
            }
        });

        public static ReplyKeyboardMarkup FAQ = new ReplyKeyboardMarkup(new KeyboardButton("Назад"));

        public static ReplyKeyboardMarkup Ideas = new ReplyKeyboardMarkup(new KeyboardButton("Назад"));
    }
}