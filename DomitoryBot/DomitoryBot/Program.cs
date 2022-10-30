// See https://aka.ms/new-console-template for more information

using Telegram;

static string GetTokenFromFile()
{
    var path = Directory.GetCurrentDirectory()
        .Replace("bin\\Debug\\net6.0", "TelegramFiles\\token.txt");
    var reader = new StreamReader(path);
    var token = reader.ReadToEnd();
    return token;
}


var c = new TelegramBotCore();
var token = GetTokenFromFile();
await c.StartBot(token);


