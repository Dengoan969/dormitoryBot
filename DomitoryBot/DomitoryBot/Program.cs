// See https://aka.ms/new-console-template for more information

using Telegram;

static string GetToken()
{
    return Console.ReadLine();
}


var c = new TelegramBotCore();
var token = GetToken();
await c.StartBot(token);