using LinksKeeperTelegramBot.Router;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LinksKeeperTelegramBot.Service;

public class LogicManager
{
    private Dictionary<State, Func<int>> _stateLogicPairs;

    public LogicManager()
    {
        _stateLogicPairs = new Dictionary<State, Func<int>>();
    }

    public Task ProcessBotUpdate(long chatId, TransmittedData transmittedData, Update update, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        return null;
    }
}