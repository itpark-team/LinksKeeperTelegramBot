using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Service.MenuPoints;

public class SharedServices
{
    public static BotTextMessage GotoProcessClickInMenuMain(TransmittedData transmittedData)
    {
        transmittedData.State = State.ClickInMenuMain;
        transmittedData.DataStorage.Clear();
        
        return new BotTextMessage(
            DialogsStringsStorage.MenuMain,
            InlineKeyboardsMarkupStorage.MenuMain
        );
    }
}