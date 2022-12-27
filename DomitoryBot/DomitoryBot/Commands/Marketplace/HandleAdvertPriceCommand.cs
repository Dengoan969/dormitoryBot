using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands.Marketplace;

public class HandleAdvertPriceCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleAdvertPriceCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Marketplace_Price;
    public DialogState DestinationState => DialogState.Marketplace_Time;


    public async Task HandleMessage(Message message, long chatId)
    {
        if (message.Text != null)
        {
            dialogManager.Value.temp_input[chatId].Add(message.Text);
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                  "На сколько дней разместить объявление?", Keyboard.Back);
        }
        else
        {
            await dialogManager.Value.ChangeState(SourceState, chatId,
                                                  "Напиши что предложишь в награду", Keyboard.Back);
        }
    }
}