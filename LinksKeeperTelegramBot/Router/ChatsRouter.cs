using LinksKeeperTelegramBot.BotInitializer;
using LinksKeeperTelegramBot.Service;
using LinksKeeperTelegramBot.Service.MenuPoints;
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
        
        Logger.Info($"ROUTER chatId = {chatId}; State = {transmittedData.State}");

        if (textData == SystemStringsStorage.CommandReset && transmittedData.State != State.CommandStart)
        {
            return SharedService.GotoProcessClickInMenuMain(transmittedData);
        }
        
        return _servicesManager.ProcessBotUpdate(textData, transmittedData);
    }
}