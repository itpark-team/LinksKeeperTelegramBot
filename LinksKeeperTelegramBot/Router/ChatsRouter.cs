using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Service;
using LinksKeeperTelegramBot.Service.Services;
using LinksKeeperTelegramBot.Util;
using NLog;

namespace LinksKeeperTelegramBot.Router;

public class ChatsRouter
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private Dictionary<long, TransmittedData> _chatTransmittedDataPairs;
    private ServicesManager _servicesManager;

    public ChatsRouter()
    {
        _chatTransmittedDataPairs = new Dictionary<long, TransmittedData>();
        _servicesManager = new ServicesManager();
    }

    public BotTextMessage Route(long chatId, string textData)
    {
        if (!_chatTransmittedDataPairs.ContainsKey(chatId))
        {
            _chatTransmittedDataPairs[chatId] = new TransmittedData(chatId);
        }

        TransmittedData transmittedData = _chatTransmittedDataPairs[chatId];

        //process reset command
        if (textData == SystemStringsStorage.CommandReset && transmittedData.State != State.WaitingCommandStart)
        {
            return SharedServices.ProcessCommandReset(transmittedData);
        }
        
        return _servicesManager.ProcessBotUpdate(textData, transmittedData);
    }
}