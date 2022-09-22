using LinksKeeperTelegramBot.Model;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace LinksKeeperTelegramBot.BotSettings;

public class Bot
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

    public Bot()
    {
        Logger.Info("Старт инициализации TelegramBotClient");

        _botClient = new TelegramBotClient("5480525378:AAEY5Ba6XBhG-q7FuA8CrP62V1i3916t1Zs");
        _cancellationTokenSource = new CancellationTokenSource();

        Logger.Info("Выполнена инициализация TelegramBotClient с токеном 5480525378:AAEY5Ba6XBhG-q7FuA8CrP62V1i3916t1Zs");
    }

    public void Start()
    {
        Logger.Info("Старт инициализации ReceiverOptions и BotRequestHandlers");
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

        Logger.Info("Выполнена инициализация ReceiverOptions и BotRequestHandlers и выполнен TelegramBotClient StartReceiving");
    }

    public string GetBotName()
    {
        Logger.Info("Старт получения имени бота");
        string userName = _botClient.GetMeAsync().Result.Username;
        Logger.Info("Выполнено получение имени бота");
        return userName;
    }

    public void Stop()
    {
        Logger.Info("Старт остановки бота");
        _cancellationTokenSource.Cancel();
        Logger.Info("Выполнено остановка бота");
    }
}