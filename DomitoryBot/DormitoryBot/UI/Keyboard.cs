using DormitoryBot.App;
using Telegram.Bot.Types.ReplyMarkups;

namespace DormitoryBot.UI;

public static class Keyboard
{
    private static InlineKeyboardMarkup Menu = new(new[]
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

    private static InlineKeyboardMarkup Washing = new(new[]
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


    private static InlineKeyboardMarkup Marketplace = new(new[]
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

    private static InlineKeyboardMarkup Back = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Назад", "Back")
        }
    });

    private static InlineKeyboardMarkup Subscriptions = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData("Подписаться", "Subscribe"),
            InlineKeyboardButton.WithCallbackData("Отписаться", "Unsubscribe"),
            InlineKeyboardButton.WithCallbackData("Все рассылки", "AllSubscriptions")
        },

        new[]
        {
            InlineKeyboardButton.WithCallbackData("Мои подписки", "MySubscriptions"),
            InlineKeyboardButton.WithCallbackData("Управление рассылками", "SubscriptionsManage"),
            InlineKeyboardButton.WithCallbackData("Назад", "Back")
        }
    });

    private static InlineKeyboardMarkup SubscriptionsManage = new(new[]
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

    private static readonly Dictionary<DialogState, InlineKeyboardMarkup> stateToKeyboard = new()
    {
        {DialogState.Menu, Menu},
        {DialogState.Marketplace, Marketplace},
        {DialogState.Washing, Washing},
        {DialogState.Subscriptions, Subscriptions},
        {DialogState.SubscriptionsManage, SubscriptionsManage}
    };


    public static InlineKeyboardMarkup GetKeyboardByState(DialogState dialogState)
    {
        return stateToKeyboard.ContainsKey(dialogState) ? stateToKeyboard[dialogState] : Back;
    }
}