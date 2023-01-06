using DormitoryBot.App.Commands.Interfaces;
using DormitoryBot.App.Interfaces;
using DormitoryBot.UI;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DormitoryBot.App
{
    public class TelegramDialogManager : ITelegramDialogSender, ITelegramUpdateHandler
    {
        private readonly TelegramBotClient botClient;
        private readonly Dictionary<DialogState, IChatCommand[]> stateCommands;
        private readonly IUsersStateRepository usersStateRepository;

        public TelegramDialogManager(TelegramBotClient botClient,
            IChatCommand[] commands, IUsersStateRepository usersStateRepository)
        {
            this.botClient = botClient;
            stateCommands = new Dictionary<DialogState, IChatCommand[]>();
            foreach (var state in Enum.GetValues<DialogState>())
            {
                stateCommands[state] = commands
                    .Where(x => x.SourceState == state || x.SourceState == DialogState.None
                        && state != DialogState.Start
                        && state != DialogState.Menu)
                    .ToArray();
            }

            TempInput = new Dictionary<long, List<object>>();
            this.usersStateRepository = usersStateRepository;
        }

        public Dictionary<long, List<object>> TempInput { get; }

        public async Task SendTextMessageAsync(long chatId, string message)
        {
            await botClient.SendTextMessageAsync(chatId, message);
        }

        public async Task SendPhotoAsync(long chatId, string photoId, string? caption = null)
        {
            await botClient.SendPhotoAsync(chatId, photoId, caption);
        }

        public async Task SendTextMessageWithChangingStateAsync(long chatId, string message,
            DialogState newState)
        {
            usersStateRepository.SetState(chatId, newState);
            var keyboard = Keyboard.GetKeyboardByState(newState);
            await botClient.SendTextMessageAsync(chatId, message, replyMarkup: keyboard);
        }

        public async Task HandleUpdate(Update update)
        {
            if (update.Message?.Date >= DateTime.Now - TimeSpan.FromSeconds(10))
            {
                return;
            }

            var chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message.Chat.Id;
            if (!chatId.HasValue)
            {
                return;
            }

            if (!usersStateRepository.ContainsKey(chatId.Value))
            {
                await SendTextMessageWithChangingStateAsync(chatId.Value, "Меню", DialogState.Menu);
            }
            else
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        var input = update.Message;
                        await TryHandleMessage(input, chatId.Value);
                        //await BotClient.DeleteMessageAsync(chatId, update.Message.MessageId);
                        break;
                    case UpdateType.CallbackQuery:
                        var command = update.CallbackQuery.Data;
                        await TryExecuteCommand(command, chatId.Value);
                        await botClient.DeleteMessageAsync(chatId, update.CallbackQuery.Message.MessageId);
                        break;
                }
            }
        }

        private async Task TryExecuteCommand(string commandName, long chatId)
        {
            var command = stateCommands[usersStateRepository.GetState(chatId)]
                .FirstOrDefault(x => x is IExecutableCommand command && command.Name == commandName);
            if (command != null)
            {
                await ((IExecutableCommand) command).Execute(chatId);
            }
        }

        private async Task TryHandleMessage(Message message, long chatId)
        {
            var command =
                stateCommands[usersStateRepository.GetState(chatId)].FirstOrDefault(x => x is IHandleTextCommand);
            if (command != null)
            {
                await ((IHandleTextCommand) command).HandleMessage(message, chatId);
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId, "Прости, но я тебя не понял :(");
            }
        }
    }
}