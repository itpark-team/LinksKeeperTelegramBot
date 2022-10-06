using LinksKeeperTelegramBot.Router;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LinksKeeperTelegramBot.BotSettings;

public class BotRequestHandlers
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    private ChatsRouter _chatsRouter;

    public BotRequestHandlers()
    {
        Logger.Info("Старт инициализации ChatsRouter");
        _chatsRouter = new ChatsRouter();
        Logger.Info("Выволнена инициализация ChatsRouter");
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        Logger.Info("Старт обработки входящего сообщения от клиента в методе HandleUpdateAsync");
        
        long chatId = 0;
        int messageId = 0;
        
        bool canRoute = false;
        
        switch (update.Type)
        {
            case UpdateType.Message:
                if (update.Message != null)
                {
                    canRoute = true;
                    chatId = update.Message.Chat.Id;
                    messageId = update.Message.MessageId;
                    Logger.Debug($"Тип входящего сообщения от chatId = {chatId} - UpdateType.Message");
                }

                break;

            case UpdateType.CallbackQuery:
                if (update.CallbackQuery != null)
                {
                    canRoute = true;
                    chatId = update.CallbackQuery.Message.Chat.Id;
                    messageId = update.CallbackQuery.Message.MessageId;
                    Logger.Debug($"Тип входящего сообщения chatId = {chatId} - UpdateType.CallbackQuery");
                }

                break;
        }
        
        if (canRoute)
        {
            try
            {
                await _chatsRouter.Route(chatId, update, botClient, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine("check Exception: "+e.ToString());

                await botClient.DeleteMessageAsync(
                    chatId:chatId, 
                    messageId:messageId, 
                    cancellationToken:cancellationToken);
                
                Console.WriteLine("error message deleted");
            }
        }
        
        Logger.Info($"Выполенна обработка входящего сообщения от chatId = {chatId} в методе HandleUpdateAsync");

    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        string errorMessage = "empty";
        switch (exception)
        {
            case ApiRequestException:
            {
                var ex = exception as ApiRequestException;
                errorMessage = $"Telegram API Error:\n[{ex.ErrorCode}]\n{ex.Message}";
            }
                break;
            default:
            {
                errorMessage = exception.ToString();
            }
                break;
        }

        Logger.Error($"Ошибка поймана в методе HandlePollingErrorAsync, {errorMessage}");
        return Task.CompletedTask;
    }
}