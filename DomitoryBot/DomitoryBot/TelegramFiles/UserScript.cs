using DomitoryBot.Commands;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Ninject.Modules;

namespace Telegram
{
    //[Flags]
    public enum DialogState
    {
        Start,
        Menu,
        Back,
        Washing,
        Washing_Date,
        Washing_Machine,
        Marketplace,
        Subscriptions,
        FAQ,
        Ideas
    }

    public class DialogManager
    {
        Func<ITelegramBotClient, Update, Task> toPrevious;
        DialogState state;
        public readonly TelegramBotClient BotClient;
        public readonly Dictionary<DialogState, IChatCommand[]> commands;

        public DialogManager(TelegramBotClient botClient)
        {
            this.BotClient = botClient;
            commands = new Dictionary<DialogState, IChatCommand[]> {
                { DialogState.Start, new IChatCommand[] { new ToMenuCommand(this) } },
                { DialogState.Menu, new IChatCommand[] {
                    new ToWashingCommand(this), new ToMarketplaceCommand(this),
                    new ToSubscriptionsCommand(this), new ToFAQCommand(this),
                    new ToIdeasCommand(this) } },
                {DialogState.Washing, new IChatCommand[] {
                    new ToFreeSlotsCommand(this), new MyEntriesCommand(this),
                new BackCommand(this), new CreateEntryCommand(this), new DeleteEntryCommand(this)} }
            };
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
                var command = update.CallbackQuery?.Data;
                var input = update.Message?.Text;
                if(command != null)
                {
                    await TryExecuteCommand(command, chatId.Value);
                }
                else if (input != null)
                {
                    await TryHandleText(input, chatId.Value);
                }
            }
            if (update.CallbackQuery != null)
            {
                await BotClient.DeleteMessageAsync(chatId, update.CallbackQuery.Message.MessageId);
            }
            if (update.Message != null)
            {
                await BotClient.DeleteMessageAsync(chatId, update.Message.MessageId);
            }
        }

        private async Task TryExecuteCommand(string commandName, long chatId)
        {
            var command = (IExecutableCommand) commands[state]
                .First(x => x is IExecutableCommand command && command.Name == commandName);
            await command.Execute(chatId);
        }

        private async Task TryHandleText(string text, long chatId)
        {
            var command = (IHandleTextCommand)commands[state].First(x => x is IHandleTextCommand);
            await command.HandleText(text, chatId);
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

        public static InlineKeyboardMarkup Washing_Date = new InlineKeyboardMarkup(new[] {
            new [] {InlineKeyboardButton.WithCallbackData("Сегодня","FreeSlots"),
                    InlineKeyboardButton.WithCallbackData("Завтра","MyEntries"),
                    InlineKeyboardButton.WithCallbackData("Послезавтра","Back"),
                    InlineKeyboardButton.WithCallbackData("Назад","Back")} });

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
