using LinksKeeperTelegramBot.Service;
using LinksKeeperTelegramBot.Service.Handlers;
using LinksKeeperTelegramBot.Util;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

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

    public Task Route(long chatId, Update update, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        Logger.Info($"Старт метода Route для chatId = {chatId}");
        if (!_chatTransmittedDataPairs.ContainsKey(chatId))
        {
            _chatTransmittedDataPairs[chatId] = new TransmittedData(chatId);
        }

        TransmittedData transmittedData = _chatTransmittedDataPairs[chatId];

        //process reset command
        if (update.Message != null && update.Message.Text == SystemStringsStorage.CommandReset && transmittedData.State != State.WaitingCommandStart)
        {
            return GlobalServices.ProcessCommandReset(chatId, transmittedData, botClient, cancellationToken);
        }
        
        Task task = _servicesManager.ProcessBotUpdate(chatId, transmittedData, update, botClient, cancellationToken);

        Logger.Info($"Выполнен метода Route для chatId = {chatId}");

        return task;
    }
}