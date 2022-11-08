using LinksKeeperTelegramBot.Model;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace LinksKeeperTelegramBot.BotInitializer;

public class Bot
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

    public Bot()
    {
        _botClient = new TelegramBotClient("5480525378:AAEY5Ba6XBhG-q7FuA8CrP62V1i3916t1Zs");
        _cancellationTokenSource = new CancellationTokenSource();

        Logger.Info(
            "Выполнена инициализация Бота с токеном 5480525378:AAEY5Ba6XBhG-q7FuA8CrP62V1i3916t1Zs");
    }

    public void Start()
    {
        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        BotRequestHandlers botRequestHandlers = new BotRequestHandlers();

        _botClient.StartReceiving(
            botRequestHandlers.HandleUpdateAsync,
            botRequestHandlers.HandlePollingErrorAsync,
            receiverOptions,
            _cancellationTokenSource.Token
        );

        Logger.Info("Бот запущен");
    }

    public string GetBotName()
    {
        return _botClient.GetMeAsync().Result.Username;
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
        Logger.Info("Бот остановлен");
    }
}