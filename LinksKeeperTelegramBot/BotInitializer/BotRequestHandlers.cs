using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
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
        string textData = SystemStringsStorage.Empty;

        bool canRoute = false;

        switch (update.Type)
        {
            case UpdateType.Message:
                if (update.Message != null)
                {
                    canRoute = true;
                    chatId = update.Message.Chat.Id;
                    messageId = update.Message.MessageId;
                    textData = update.Message.Text;
                    Logger.Debug($"Тип входящего сообщения от chatId = {chatId} - UpdateType.Message");
                }

                break;

            case UpdateType.CallbackQuery:
                if (update.CallbackQuery != null)
                {
                    canRoute = true;
                    chatId = update.CallbackQuery.Message.Chat.Id;
                    messageId = update.CallbackQuery.Message.MessageId;
                    textData = update.CallbackQuery.Data;
                    Logger.Debug($"Тип входящего сообщения chatId = {chatId} - UpdateType.CallbackQuery");
                }

                break;
        }

        if (canRoute)
        {
            try
            {
                BotTextMessage botTextMessage =
                    await Task.Run(() => _chatsRouter.Route(chatId, textData), cancellationToken);

                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: botTextMessage.Text,
                    replyMarkup: botTextMessage.InlineKeyboardMarkup,
                    cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                await botClient.DeleteMessageAsync(
                    chatId: chatId,
                    messageId: messageId,
                    cancellationToken: cancellationToken);
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