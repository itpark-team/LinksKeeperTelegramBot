using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util.InlineKeyboardsMarkupInitializer;
using LinksKeeperTelegramBot.Util.ReplyTextsInitializer;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Service.SharedProcessors;

public class GlobalServices
{
    public static Task ProcessCommandReset(long chatId, TransmittedData transmittedData, ITelegramBotClient botClient,
        CancellationToken cancellationToken)
    {
        transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;
        transmittedData.ClearDataStorage();
        
        string responseMessageText = ReplyTextsStorage.MenuMain;

        InlineKeyboardMarkup responseInlineKeyboardMarkup = InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain;

        Task taskSuccess = botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            replyMarkup: responseInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
        
        return taskSuccess;
    }
}