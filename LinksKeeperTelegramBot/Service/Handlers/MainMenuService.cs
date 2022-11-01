using System.Text;
using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
using NLog;

namespace LinksKeeperTelegramBot.Service.Handlers;

public class MainMenuService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    public BotTextMessage ProcessCommandStart(string command, TransmittedData transmittedData)
    {
        if (command != SystemStringsStorage.CommandStart)
        {
            return new BotTextMessage(DialogsStringsStorage.CommandStartInputErrorInput);
        }

        transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;

        return new BotTextMessage(
            DialogsStringsStorage.MenuMain,
            InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain
        );
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
        else if (callBackData == BotButtonsStorage.ButtonHowToUseInMenuMain.CallBackData)
        {
            return new BotTextMessage("Нажата клавиша Как пользоваться");
        }

        throw new Exception("Bad user request");
    }

    public BotTextMessage ProcessClickOnInlineButtonInMenuAdd(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonLinkInMenuAdd.CallBackData)
        {
            transmittedData.State = State.WaitingInputLinkUrlForAdd;

            return new BotTextMessage(DialogsStringsStorage.LinkInputUrl);
        }
        else if (callBackData == BotButtonsStorage.ButtonCategoryInMenuAdd.CallBackData)
        {
            IEnumerable<LinkCategory> linkCategories =
                DbManager.GetInstance().TableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            string replyText = DialogsStringsStorage.MenuCategoryStart;

            StringBuilder stringBuilder = new StringBuilder();
            foreach (LinkCategory linkCategory in linkCategories)
            {
                stringBuilder.AppendLine(linkCategory.Name);
            }

            replyText += stringBuilder + "\n";

            replyText += DialogsStringsStorage.CreateMenuCategoryHowManyUseCategories(linkCategories.Count(),
                Constants.MaxLinksCategoriesCount);

            if (linkCategories.Count() < Constants.MaxLinksCategoriesCount)
            {
                replyText += DialogsStringsStorage.MenuCategoryInputNew;
                
                transmittedData.State = State.WaitingInputCategoryForAdd;
                
                return new BotTextMessage(replyText);
            }
            else
            {
                replyText += DialogsStringsStorage.MenuCategoryInputRestrict;
                
                transmittedData.State = State.WaitingClickOnInlineButtonInMenuAddCategory;
                
                return new BotTextMessage(
                    replyText,
                    InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuAddCategory
                );
            }
        }
        else if (callBackData == BotButtonsStorage.ButtonBackwardInMenuAdd.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;

            return new BotTextMessage(
                DialogsStringsStorage.MenuMain,
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain
            );
        }

        throw new Exception("Bad user request");
    }
    
    public BotTextMessage ProcessClickOnInlineButtonInMenuDelete(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonLinkInMenuDelete.CallBackData)
        {
            if (DbManager.GetInstance().TableLinksCategories.ContaintByChatId(transmittedData.ChatId) == false)
            {
                return new BotTextMessage(DialogsStringsStorage.MenuDeleteNoCategories);
            }

            transmittedData.State = State.WaitingClickOnInlineButtonLinkCategoryForDeleteLinks;

            TableLinksCategories tableLinksCategories = DbManager.GetInstance().TableLinksCategories;

            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuDeleteLink,
                InlineKeyboardsMarkupStorage.CreateInlineKeyboardMarkupMenuLinkCategoryForDeleteLink(linkCategories)
            );
            
        }
        else if (callBackData == BotButtonsStorage.ButtonCategoryInMenuDelete.CallBackData)
        {
            if (DbManager.GetInstance().TableLinksCategories.ContaintByChatId(transmittedData.ChatId) == false)
            {
                return new BotTextMessage(DialogsStringsStorage.MenuShowNoCategories);
            }

            transmittedData.State = State.WaitingClickOnInlineButtonMenuDeleteCategory;

            TableLinksCategories tableLinksCategories = DbManager.GetInstance().TableLinksCategories;

            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuDeleteCategory,
                InlineKeyboardsMarkupStorage.CreateInlineKeyboardMarkupMenuDeleteCategory(linkCategories)
            );
        }
        else if (callBackData == BotButtonsStorage.ButtonBackwardInMenuDelete.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;

            return new BotTextMessage(
                DialogsStringsStorage.MenuMain,
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain
            );
        }

        throw new Exception("Bad user request");
    }
}