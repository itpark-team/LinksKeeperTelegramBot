using LinksKeeperTelegramBot.BotSettings;

Bot bot = new Bot();
bot.Start();

Console.WriteLine($"Bot @{bot.GetBotName()} started");

Console.WriteLine("Press enter for stop");
Console.ReadKey();

bot.Stop();

Console.WriteLine("Bot stopped");







