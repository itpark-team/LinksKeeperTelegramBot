using LinksKeeperTelegramBot.Service;
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
            _chatTransmittedDataPairs[chatId] = new TransmittedData();
        }

        TransmittedData transmittedData = _chatTransmittedDataPairs[chatId];

        Task task = _servicesManager.ProcessBotUpdate(chatId, transmittedData, update, botClient, cancellationToken);

        Logger.Info($"Выполнен метода Route для chatId = {chatId}");

        return task;
    }
}