using LinksKeeperTelegramBot.Service;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LinksKeeperTelegramBot.Router;

public class ChatsRouter
{
    private Dictionary<long, TransmittedData> _chatTransmittedDataPairs;
    private LogicManager _logicManager;

    public ChatsRouter()
    {
        _chatTransmittedDataPairs = new Dictionary<long, TransmittedData>();
        _logicManager = new LogicManager();
    }

    public Task Route(long chatId, Update update, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        if (!_chatTransmittedDataPairs.ContainsKey(chatId))
        {
            _chatTransmittedDataPairs[chatId] = new TransmittedData();
        }

        TransmittedData transmittedData = _chatTransmittedDataPairs[chatId];

        return _logicManager.ProcessBotUpdate(chatId, transmittedData, update, botClient, cancellationToken);
    }
}