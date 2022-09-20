using LinksKeeperTelegramBot.Model;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace LinksKeeperTelegramBot.BotSettings;

public class Bot
{
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

    public Bot()
    {
        _botClient = new TelegramBotClient("5480525378:AAEY5Ba6XBhG-q7FuA8CrP62V1i3916t1Zs");
        _cancellationTokenSource = new CancellationTokenSource();
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
    }

    public string GetBotName()
    {
        return _botClient.GetMeAsync().Result.Username;
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
    }
}