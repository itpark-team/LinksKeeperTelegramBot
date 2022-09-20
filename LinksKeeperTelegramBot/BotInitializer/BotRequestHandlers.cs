using LinksKeeperTelegramBot.Router;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LinksKeeperTelegramBot.BotSettings;

public class BotRequestHandlers
{
    private ChatsRouter _chatsRouter;

    public BotRequestHandlers()
    {
        _chatsRouter = new ChatsRouter();
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        long chatId = 0;
        bool canRoute = false;

        switch (update.Type)
        {
            case UpdateType.Message:
                if (update.Message != null)
                {
                    canRoute = true;
                    chatId = update.Message.Chat.Id;
                }
                break;

            case UpdateType.CallbackQuery:
                if (update.CallbackQuery != null)
                {
                    canRoute = true;
                    chatId = update.CallbackQuery.Message.Chat.Id;
                }

                break;
        }

        if (canRoute)
        {
            await _chatsRouter.Route(chatId, update, botClient, cancellationToken);
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}