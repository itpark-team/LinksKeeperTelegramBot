using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Service.Services;

public class SharedServices
{
    public static BotTextMessage ProcessCommandReset(TransmittedData transmittedData)
    {
        transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;
        transmittedData.DataStorage.Clear();

        return new BotTextMessage(DialogsStringsStorage.MenuMain,
            InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain);
    }

    public static BotTextMessage GotoProcessClickOnInlineButtonInMenuMain(TransmittedData transmittedData)
    {
        transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;

        return new BotTextMessage(
            DialogsStringsStorage.MenuMain,
            InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain
        );
    }
}