using LinksKeeperTelegramBot.BotInitializer;

Bot bot = new Bot();
bot.Start();

TaskCompletionSource tcs = new TaskCompletionSource();

AppDomain.CurrentDomain.ProcessExit += (_, _) =>
{
    bot.Stop();
    Console.WriteLine("Bot stopped");
    tcs.SetResult();
};

Console.WriteLine($"Bot @{bot.GetBotName()} started");
Console.WriteLine("Press Ctrl+C for stop");

await tcs.Task;

Console.WriteLine("program finished");









