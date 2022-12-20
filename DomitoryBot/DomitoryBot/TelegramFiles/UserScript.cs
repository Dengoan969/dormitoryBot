using DomitoryBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
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
        Func<ITelegramBotClient, Update, Task> toPrevious;
        DialogState state;
        public readonly TelegramBotClient BotClient;
        public readonly Dictionary<DialogState, IChatCommand[]> stateCommands;

        public DialogManager(TelegramBotClient botClient, IChatCommand[] commands)
        {
            this.BotClient = botClient;
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
            if (state == DialogState.Start)
            {
                await ChangeState(DialogState.Menu, chatId, "Меню", Keyboard.Menu);
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
            var command = (IExecutableCommand)stateCommands[state]
                .First(x => x is IExecutableCommand command && command.Name == commandName);
            await command.Execute(chatId);
        }

        private async Task TryHandleMessage(Message message, long chatId)
        {
            var command = (IHandleTextCommand)stateCommands[state].First(x => x is IHandleTextCommand);
            await command.HandleMessage(message, chatId);
        }

        public async Task ChangeState(DialogState newState, ChatId chatId, string message, IReplyMarkup keyboard)
        {
            state = newState;
            await BotClient.SendTextMessageAsync(chatId, message, replyMarkup: keyboard);
        }
    }

    public static class Keyboard
    {
        public static InlineKeyboardMarkup Menu = new InlineKeyboardMarkup(new[] {
            new [] {InlineKeyboardButton.WithCallbackData("Стирка","Washing"),
                    InlineKeyboardButton.WithCallbackData("Маркетплейс","Marketplace"),
                    InlineKeyboardButton.WithCallbackData("Объявления","Subscriptions")},

            new [] {InlineKeyboardButton.WithCallbackData("FAQ", "FAQ"),
                    InlineKeyboardButton.WithCallbackData("Предложить идею","Ideas")} });

        //public static ReplyKeyboardMarkup Back = new ReplyKeyboardMarkup(new KeyboardButton("Назад"));

        public static InlineKeyboardMarkup Washing = new InlineKeyboardMarkup(new[] {
            new [] {InlineKeyboardButton.WithCallbackData("Свободные слоты","FreeSlots"),
                    InlineKeyboardButton.WithCallbackData("Мои записи","MyEntries"),
                    InlineKeyboardButton.WithCallbackData("Назад","Back")},

            new [] {InlineKeyboardButton.WithCallbackData("Записаться", "CreateEntry"),
                    InlineKeyboardButton.WithCallbackData("Удалить запись","DeleteEntry")} });

        //public static InlineKeyboardMarkup Washing_Date = new InlineKeyboardMarkup(new[] {
        //    new [] {InlineKeyboardButton.WithCallbackData("Сегодня","FreeSlots"),
        //            InlineKeyboardButton.WithCallbackData("Завтра","MyEntries"),
        //            InlineKeyboardButton.WithCallbackData("Послезавтра","Back"),
        //            InlineKeyboardButton.WithCallbackData("Назад","Back")} });

        public static InlineKeyboardMarkup Washing_Machine = new InlineKeyboardMarkup(new[] {
            new [] {InlineKeyboardButton.WithCallbackData("1","FreeSlots"),
                    InlineKeyboardButton.WithCallbackData("2","MyEntries"),
                    InlineKeyboardButton.WithCallbackData("3","Back"),
                    InlineKeyboardButton.WithCallbackData("Назад","Back")} });

        public static InlineKeyboardMarkup Marketplace = new InlineKeyboardMarkup(new[] {
            new [] {InlineKeyboardButton.WithCallbackData("Все объявления","FreeSlots"),
                    InlineKeyboardButton.WithCallbackData("Мои объявления","MyEntries")},

            new [] {InlineKeyboardButton.WithCallbackData("Создать объявление", "CreateEntry"),
                    InlineKeyboardButton.WithCallbackData("Назад","DeleteEntry")} });

        public static InlineKeyboardMarkup Subscriptions = new InlineKeyboardMarkup(new[] {
            new [] {InlineKeyboardButton.WithCallbackData("Подписаться","FreeSlots"),
                    InlineKeyboardButton.WithCallbackData("Отписаться","MyEntries")},

            new [] {InlineKeyboardButton.WithCallbackData("Создать объявление", "CreateEntry"),
                    InlineKeyboardButton.WithCallbackData("Назад","DeleteEntry")} });

        public static ReplyKeyboardMarkup FAQ = new ReplyKeyboardMarkup(new KeyboardButton("Назад"));

        public static ReplyKeyboardMarkup Ideas = new ReplyKeyboardMarkup(new KeyboardButton("Назад"));
    }
}
