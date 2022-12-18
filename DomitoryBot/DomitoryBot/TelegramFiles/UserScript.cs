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
                    new ToIdeasCommand(this) } }
            };
        }

        public async Task HandleUpdate(Update update)
        {
            var text = update.Message?.Text ?? update.CallbackQuery?.Data;
            var chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message.Chat.Id;
            if(!chatId.HasValue)
            {
                return;
            }
            if(state == DialogState.Start)
            {
                await StateMenu(chatId.Value);
            }
            else
            {
                await TryExecuteCommand(text, "", chatId.Value);
                if(update.CallbackQuery!=null)
                {
                    await BotClient.DeleteMessageAsync(chatId, update.CallbackQuery.Message.MessageId);
                }
                
            }
        }

        private async Task TryExecuteCommand(string commandName, string text, long chatId)
        {
            foreach(var command in commands[state])
            {
                if(command.Command == commandName)
                {
                    await command.HandleText(text, chatId);
                    break;
                }
            }
        }
        public async Task StateMenu(long chatId)
        {
            state = DialogState.Menu;
            await BotClient.SendTextMessageAsync(chatId, "Меню", replyMarkup: Keyboard.Menu);
        }

        public async Task StateWashing(ChatId chatId)
        {
            state = DialogState.Washing;
            await BotClient.SendTextMessageAsync(chatId, "Стирка", replyMarkup: Keyboard.Washing);
        }

        public async Task StateMarketplace(ChatId chatId)
        {
            state = DialogState.Marketplace;
            await BotClient.SendTextMessageAsync(chatId, "Маркетплейс", replyMarkup: Keyboard.Marketplace);
        }

        public async Task StateSubscriptions(ChatId chatId)
        {
            state = DialogState.Subscriptions;
            await BotClient.SendTextMessageAsync(chatId, "Объявления", replyMarkup: Keyboard.Subscriptions);
        }
        public async Task StateFAQ(ChatId chatId)
        {
            state = DialogState.FAQ;
            await BotClient.SendTextMessageAsync(chatId, "FAQ", replyMarkup: Keyboard.FAQ);
        }
        public async Task StateIdeas(ChatId chatId)
        {
            state = DialogState.Ideas;
            await BotClient.SendTextMessageAsync(chatId, "Предложить идею", replyMarkup: Keyboard.Ideas);
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

        public static ReplyKeyboardMarkup Back = new ReplyKeyboardMarkup(new KeyboardButton("Назад"));

        public static ReplyKeyboardMarkup Washing = new ReplyKeyboardMarkup(new[] {
            new KeyboardButton[] {"Свободные слоты", "Мои записи", "Назад"},
            new KeyboardButton[] {"Записаться", "Удалить запись"} });

        public static ReplyKeyboardMarkup Washing_Date = new ReplyKeyboardMarkup(new[] {
            new KeyboardButton[] {"Сегодня", "Завтра","Послезавтра", "Назад"}});

        public static ReplyKeyboardMarkup Washing_Machine = new ReplyKeyboardMarkup(new[] {
            new KeyboardButton[] {"1", "2","3", "Назад"}});

        public static ReplyKeyboardMarkup Marketplace = new ReplyKeyboardMarkup(new[] {
            new KeyboardButton[] {"Все объявления", "Мои объявления"},
            new KeyboardButton[] {"Создать объявление", "Назад" } });

        public static ReplyKeyboardMarkup Subscriptions = new ReplyKeyboardMarkup(new[] {
            new KeyboardButton[] {"Подписаться", "Отписаться"},
            new KeyboardButton[] {"Создать объявление", "Назад" } });

        public static ReplyKeyboardMarkup FAQ = new ReplyKeyboardMarkup(new KeyboardButton("Назад"));

        public static ReplyKeyboardMarkup Ideas = new ReplyKeyboardMarkup(new KeyboardButton("Назад"));
    }
}
