using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LinksKeeperTelegramBot.BotInitializer;

public class BotRequestHandlers
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    private ChatsRouter _chatsRouter;

    public BotRequestHandlers()
    {
        _chatsRouter = new ChatsRouter();
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
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
                    
                }

                break;

            case UpdateType.CallbackQuery:
                if (update.CallbackQuery != null)
                {
                    canRoute = true;
                    chatId = update.CallbackQuery.Message.Chat.Id;
                    messageId = update.CallbackQuery.Message.MessageId;
                    textData = update.CallbackQuery.Data;
                }

                break;
        }
        
        Logger.Info($"ВХОДЯЩЕЕ СОООБЩЕНИЕ UpdateType = {update.Type}; chatId = {chatId}; messageId = {messageId}; text = {textData} canRoute = {canRoute}");

        if (canRoute)
        {
            try
            {
                BotTextMessage botTextMessage =
                    await Task.Run(() => _chatsRouter.Route(chatId, textData), cancellationToken);

                Logger.Info($"ИСХОДЯЩЕЕ СОООБЩЕНИЕ chatId = {chatId}; text = {botTextMessage.Text.Replace("\n"," ")}; Keyboard = ???");
                
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: botTextMessage.Text,
                    replyMarkup: botTextMessage.InlineKeyboardMarkup,
                    cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                Logger.Error($"ОШИБКА chatId = {chatId}; text = {e.Message}");
                await botClient.DeleteMessageAsync(
                    chatId: chatId,
                    messageId: messageId,
                    cancellationToken: cancellationToken);
            }
        }
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