using LinksKeeperTelegramBot.BotInitializer;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;


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