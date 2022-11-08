using System.Text;
using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
using NLog;

namespace LinksKeeperTelegramBot.Service.MenuPoints;

public class MainMenuService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    public BotTextMessage ProcessCommandStart(string command, TransmittedData transmittedData)
    {
        if (command != SystemStringsStorage.CommandStart)
        {
            return new BotTextMessage(DialogsStringsStorage.CommandStartInputErrorInput);
        }

        return SharedServices.GotoProcessClickInMenuMain(transmittedData);
    }

    public BotTextMessage ProcessClickInMenuMain(string callBackData, TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.AddInMenuMain.CallBackData)
        {
            transmittedData.State = State.ClickInMenuAdd;

            return new BotTextMessage(
                DialogsStringsStorage.MenuAdd,
                InlineKeyboardsMarkupStorage.MenuAdd
            );
        }
        else if (callBackData == BotButtonsStorage.ShowInMenuMain.CallBackData)
        {
            if (DbManager.GetInstance().TableLinksCategories.ContaintByChatId(transmittedData.ChatId) == false)
            {
                return new BotTextMessage(DialogsStringsStorage.MenuShowNoCategories);
            }

            transmittedData.State = State.ClickLinkCategoryShow;

            TableLinksCategories tableLinksCategories = DbManager.GetInstance().TableLinksCategories;

            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuShow,
                InlineKeyboardsMarkupStorage.CreateMenuLinkCategoryShow(linkCategories)
            );
        }
        else if (callBackData == BotButtonsStorage.DeleteInMenuMain.CallBackData)
        {
            transmittedData.State = State.ClickInMenuDelete;

            return new BotTextMessage(
                DialogsStringsStorage.MenuDelete,
                InlineKeyboardsMarkupStorage.MenuDelete
            );
        }
        throw new Exception("Bad user request");
    }
}