using LinksKeeperTelegramBot.BotInitializer;

ManualResetEvent exitEvent = new ManualResetEvent(false);

Console.CancelKeyPress += (sender, eventArgs) => {
    eventArgs.Cancel = true;
    exitEvent.Set();
};

Bot bot = new Bot();
bot.Start();

Console.WriteLine($"Bot @{bot.GetBotName()} started");
Console.WriteLine("Press Ctrl+C for stop");

exitEvent.WaitOne();

bot.Stop();










