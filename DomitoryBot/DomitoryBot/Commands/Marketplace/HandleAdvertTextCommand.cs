using DomitoryBot.App;
using DomitoryBot.Commands.Interfaces;
using DomitoryBot.UI;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands.Marketplace
{
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
            if (message.Text != null)
            {
                dialogManager.Value.temp_input[chatId] = new List<object>();
                dialogManager.Value.temp_input[chatId].Add(message.Text);
                await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                      "Напиши что просишь/предложишь в награду", Keyboard.Back);
            }
            else
            {
                await dialogManager.Value.ChangeState(SourceState, chatId,
                                                      "Напиши описание объявления", Keyboard.Back);
            }
        }
    }
}