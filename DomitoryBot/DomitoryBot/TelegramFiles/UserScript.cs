using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram
{
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

        public async Task HandleUpdate(ITelegramBotClient botClient, Update update)
        {
            var text = update.Message.Text;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            switch (state)
            {
                case DialogState.Start:
                    await StateMenu(botClient, update);
                    break;
                case DialogState.Menu:
                    switch (text)
                    {
                        case "Стирка":
                            await StateWashing(botClient, update);
                            break;
                        case "Маркетплейс":
                            await StateMarketplace(botClient, update);
                            break;
                        case "Объявления":
                            await StateSubscriptions(botClient, update);
                            break;
                        case "FAQ":
                            await StateFAQ(botClient, update);
                            break;
                        case "Предложить идею":
                            await StateIdeas(botClient, update);
                            break;
                    }
                    break;
                case DialogState.Washing:
                    switch(text)
                    {
                        case "Назад":
                            await StateMenu(botClient, update);
                            break;
                    }
                    break;
                case DialogState.Marketplace:
                    switch (text)
                    {
                        case "Назад":
                            await StateMenu(botClient, update);
                            break;
                    }
                    break;
                case DialogState.Subscriptions:
                    switch (text)
                    {
                        case "Назад":
                            await StateMenu(botClient, update);
                            break;
                    }
                    break;
                case DialogState.FAQ:
                    switch (text)
                    {
                        case "Назад":
                            await StateMenu(botClient, update);
                            break;
                    }
                    break;
                case DialogState.Ideas:
                    switch (text)
                    {
                        case "Назад":
                            await StateMenu(botClient, update);
                            break;
                    }
                    break;
            }
        }

        private async Task StateMenu(ITelegramBotClient botClient, Update update)
        {
            state = DialogState.Menu;
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Меню", replyMarkup: Keyboard.Menu);
        }

        private async Task StateWashing(ITelegramBotClient botClient, Update update)
        {
            state = DialogState.Washing;
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Стирка", replyMarkup: Keyboard.Washing);
        }

        private async Task StateMarketplace(ITelegramBotClient botClient, Update update)
        {
            state = DialogState.Marketplace;
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Маркетплейс", replyMarkup: Keyboard.Marketplace);
        }

        private async Task StateSubscriptions(ITelegramBotClient botClient, Update update)
        {
            state = DialogState.Subscriptions;
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Объявления", replyMarkup: Keyboard.Subscriptions);
        }
        private async Task StateFAQ(ITelegramBotClient botClient, Update update)
        {
            state = DialogState.FAQ;
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "FAQ", replyMarkup: Keyboard.FAQ);
        }
        private async Task StateIdeas(ITelegramBotClient botClient, Update update)
        {
            state = DialogState.Ideas;
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Предложить идею", replyMarkup: Keyboard.Ideas);
        }
    }

    public static class Keyboard
    {
        public static ReplyKeyboardMarkup Menu = new ReplyKeyboardMarkup(new[] {
            new KeyboardButton[] {"Стирка", "Маркетплейс", "Объявления"},
            new KeyboardButton[] {"FAQ", "Предложить идею"} });

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
