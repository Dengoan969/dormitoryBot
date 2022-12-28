using Telegram.Bot.Types.ReplyMarkups;

namespace DormitoryBot.App;

public interface ITelegramDialogSender : IDialogSender<IReplyMarkup, long, string>
{
}