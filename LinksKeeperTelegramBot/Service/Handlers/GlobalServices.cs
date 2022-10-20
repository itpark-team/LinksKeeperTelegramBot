using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Service.Handlers;

public class GlobalServices
{
    public static BotTextMessage ProcessCommandReset(string textData, TransmittedData transmittedData)
    {
        transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;
        transmittedData.DataStorage.Clear();

        return new BotTextMessage(DialogsStringsStorage.MenuMain,
            InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain);
    }
}