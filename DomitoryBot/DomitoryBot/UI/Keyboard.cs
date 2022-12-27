using Telegram.Bot.Types.ReplyMarkups;

namespace DomitoryBot.UI;

public static class Keyboard
{
    public static InlineKeyboardMarkup Menu = new(new[]
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

    public static InlineKeyboardMarkup Washing = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Свободные слоты", "FreeSlots"),
            InlineKeyboardButton.WithCallbackData("Мои записи", "My Records"),
            InlineKeyboardButton.WithCallbackData("Назад", "Back")
        },

        new[]
        {
            InlineKeyboardButton.WithCallbackData("Записаться", "Machine select"),
            InlineKeyboardButton.WithCallbackData("Удалить запись", "Delete record")
        }
    });

    //public static InlineKeyboardMarkup Washing_Date = new InlineKeyboardMarkup(new[] {
    //    new [] {InlineKeyboardButton.WithCallbackData("Сегодня","FreeSlots"),
    //            InlineKeyboardButton.WithCallbackData("Завтра","MyEntries"),
    //            InlineKeyboardButton.WithCallbackData("Послезавтра","Back"),
    //            InlineKeyboardButton.WithCallbackData("Назад","Back")} });

    public static InlineKeyboardMarkup Washing_Machine = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData("1", "1"),
            InlineKeyboardButton.WithCallbackData("2", "2"),
            InlineKeyboardButton.WithCallbackData("3", "3"),
            InlineKeyboardButton.WithCallbackData("Назад", "Back")
        }
    });

    public static InlineKeyboardMarkup Marketplace = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Все объявления", "AllAdverts"),
            InlineKeyboardButton.WithCallbackData("Мои объявления", "MyAdverts")
        },

        new[]
        {
            InlineKeyboardButton.WithCallbackData("Создать объявление", "CreateAdvert"),
            InlineKeyboardButton.WithCallbackData("Удалить объявление", "DeleteAdvert"),
            InlineKeyboardButton.WithCallbackData("Назад", "Back")
        }
    });

    public static InlineKeyboardMarkup Back = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Назад", "Back")
        }
    });

    public static InlineKeyboardMarkup Subscriptions = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Подписаться", "Subscribe"),
            InlineKeyboardButton.WithCallbackData("Отписаться", "Unsubscribe"),
            InlineKeyboardButton.WithCallbackData("Мои подписки", "MySubscriptions")
        },

        new[]
        {
            InlineKeyboardButton.WithCallbackData("Управление рассылками", "SubscriptionsManage"),
            InlineKeyboardButton.WithCallbackData("Назад", "Back")
        }
    });

    public static InlineKeyboardMarkup SubscriptionsManage = new(new[]
{
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Мои рассылки", "AdminSubscriptions"),
            InlineKeyboardButton.WithCallbackData("Создать рассылку", "CreateSubscription"),
            InlineKeyboardButton.WithCallbackData("Удалить рассылку", "DeleteSubscription")
        },

        new[]
        {
            InlineKeyboardButton.WithCallbackData("Опубликовать объявление", "CreateAnnouncement"),
            InlineKeyboardButton.WithCallbackData("Назад", "Back")
        }
    });
}