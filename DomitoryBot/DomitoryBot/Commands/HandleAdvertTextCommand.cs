using Telegram;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands;

public class HandleAdvertTextCommand : IHandleTextCommand
{
    private readonly Lazy<DialogManager> dialogManager;

    public HandleAdvertTextCommand(Lazy<DialogManager> dialogManager)
    {
        this.dialogManager = dialogManager;
    }

    public DialogState SourceState => DialogState.Marketplace_Text;
    public DialogState DestinationState => DialogState.Marketplace_Price;


    public async Task HandleMessage(Message message, long chatId)
    {
        if(message.Text != null)
        {
            dialogManager.Value.temp_input[chatId] = new List<object>();
            dialogManager.Value.temp_input[chatId].Add(message.Text);
            await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                  "Напиши что предожишь в награду", Keyboard.Back);
        }
        else
        {
            await dialogManager.Value.ChangeState(SourceState, chatId,
                                                  "Введите описание объявления", Keyboard.Back);
        }
    }
}