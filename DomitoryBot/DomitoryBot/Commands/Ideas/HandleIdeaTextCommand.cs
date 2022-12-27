using DomitoryBot.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace DomitoryBot.Commands.Ideas
{
    public class HandleIdeaTextCommand : IHandleTextCommand
    {
        private readonly Lazy<DialogManager> dialogManager;

        public DialogState SourceState => DialogState.Idea_Text;

        public DialogState DestinationState => DialogState.Menu;

        public HandleIdeaTextCommand(Lazy<DialogManager> dialogManager)
        {
            this.dialogManager = dialogManager;
        }
        public async Task HandleMessage(Message message, long chatId)
        {
            if (message.Text != null)
            {
                Console.WriteLine(message.Text);
                await dialogManager.Value.ChangeState(DestinationState, chatId,
                                                      "Меню", Keyboard.Menu);
            }
            else
            {
                await dialogManager.Value.ChangeState(SourceState, chatId,
                                                  "Какие есть предложения? :)", Keyboard.Back);
            }
        }
    }
}
