using System.Text;
using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Service.Services;
using LinksKeeperTelegramBot.Util;
using NLog;

namespace LinksKeeperTelegramBot.Service.Handlers;

public class MenuMainService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    public BotTextMessage ProcessCommandStart(string command, TransmittedData transmittedData)
    {
        if (command != SystemStringsStorage.CommandStart)
        {
            return new BotTextMessage(DialogsStringsStorage.CommandStartInputErrorInput);
        }

        return SharedServices.GotoProcessClickOnInlineButtonInMenuMain(transmittedData);
    }

    public BotTextMessage ProcessClickOnInlineButtonInMenuMain(string callBackData, TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonAddInMenuMain.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuAdd;

            return new BotTextMessage(
                DialogsStringsStorage.MenuAdd,
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuAdd
            );
        }
        else if (callBackData == BotButtonsStorage.ButtonShowInMenuMain.CallBackData)
        {
            if (DbManager.GetInstance().TableLinksCategories.ContaintByChatId(transmittedData.ChatId) == false)
            {
                return new BotTextMessage(DialogsStringsStorage.MenuShowNoCategories);
            }

            transmittedData.State = State.WaitingClickOnInlineButtonLinkCategoryForShow;

            TableLinksCategories tableLinksCategories = DbManager.GetInstance().TableLinksCategories;

            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuShow,
                InlineKeyboardsMarkupStorage.CreateInlineKeyboardMarkupMenuLinkCategoryForShow(linkCategories)
            );
        }
        else if (callBackData == BotButtonsStorage.ButtonDeleteInMenuMain.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuDelete;

            return new BotTextMessage(
                DialogsStringsStorage.MenuDelete,
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuDelete
            );
        }
        throw new Exception("Bad user request");
    }
}