using LinksKeeperTelegramBot.Router;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LinksKeeperTelegramBot.Service;

public class ServicesManager
{
    private Dictionary<State, Func<long, TransmittedData, Update, ITelegramBotClient, CancellationToken, Task>>
        _stateServiceMethodPairs;

    public ServicesManager()
    {
        _stateServiceMethodPairs =
            new Dictionary<State, Func<long, TransmittedData, Update, ITelegramBotClient, CancellationToken, Task>>();
        
    }

    public Task ProcessBotUpdate(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        var serviceMethod = _stateServiceMethodPairs[transmittedData.State];
        return serviceMethod.Invoke(chatId, transmittedData, update, botClient, cancellationToken);
    }
}