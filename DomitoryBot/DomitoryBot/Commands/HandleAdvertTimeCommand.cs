using Telegram;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands;

public class HandleAdvertTimeCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleAdvertTimeCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Marketplace_Time;
    public DialogState DestinationState => DialogState.Marketplace;


    public async Task HandleMessage(Message message, long chatId)
    {
        if(message.Text != null)
        {
            var temp_input = dialogManager.Value.temp_input[chatId];
            dialogManager.Value.MarketPlace.CreateAdvert(chatId, (string)temp_input[0], (string)temp_input[1], TimeSpan.FromDays(double.Parse(message.Text)));
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                  "Маркетплейс", Keyboard.Marketplace);
        }
        else
        {
            await dialogManager.Value.ChangeState(SourceState, chatId,
                                                  "На сколько дней разместить объявление?", Keyboard.Back);
        }
    }
}