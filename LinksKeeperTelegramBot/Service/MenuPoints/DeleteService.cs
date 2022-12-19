using System.Text;
using LinksKeeperTelegramBot.BotInitializer;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Service.MenuPoints;

public class DeleteService
{
    private DbManager _dbManager;

    public DeleteService(DbManager dbManager)
    {
        _dbManager = dbManager;
    }

    #region CommonMethods

    private BotTextMessage LinksToText(TransmittedData transmittedData, IEnumerable<Link> links)
    {
        StringBuilder stringBuilder = new StringBuilder();

        bool hasMessageFit = true;

        List<Link> fitLinks = new List<Link>();

        foreach (Link link in links)
        {
            int stringBuilderTextLength = stringBuilder.Length;
            int currentLinkTextLength = SystemStringsStorage.LinkId.Length + link.Id.ToString().Length +
                                        link.Url.Length + link.Description.Length +
                                        SystemStringsStorage.Devider.Length + Constants.EnterSymbolsLength;

            if (stringBuilderTextLength + currentLinkTextLength > Constants.MaxBotTextMessageLength)
            {
                hasMessageFit = false;
                transmittedData.DataStorage.AddOrUpdate(
                    SystemStringsStorage.DataStorageKeyShowLinksStartLinkIdForDelete,
                    link.Id);
                break;
            }

            stringBuilder.AppendLine(SystemStringsStorage.LinkId + link.Id);
            stringBuilder.AppendLine(link.Url);
            stringBuilder.AppendLine(link.Description);
            stringBuilder.AppendLine(SystemStringsStorage.Devider);

            fitLinks.Add(link);
        }

        string linksAsText = stringBuilder.ToString();

        InlineKeyboardMarkup inlineKeyboardMarkup = hasMessageFit
            ? InlineKeyboardsMarkupStorage.CreateLinksAllDelete(fitLinks)
            : InlineKeyboardsMarkupStorage.CreateLinksAllMoreDelete(fitLinks);

        return new BotTextMessage(
            linksAsText,
            inlineKeyboardMarkup
        );
    }

    #endregion

    public BotTextMessage ProcessClickInMenuDelete(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.LinkInMenuDelete.CallBackData)
        {
            if (_dbManager.TableLinksCategories.ContaintByChatId(transmittedData.ChatId) == false)
            {
                return new BotTextMessage(DialogsStringsStorage.MenuDeleteNoCategories);
            }

            transmittedData.State = State.ClickLinkCategoryLinksDelete;

            ITableLinksCategories tableLinksCategories = _dbManager.TableLinksCategories;

            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuDeleteLink,
                InlineKeyboardsMarkupStorage.CreateMenuLinkCategoryLinkDelete(linkCategories)
            );
        }
        else if (callBackData == BotButtonsStorage.CategoryInMenuDelete.CallBackData)
        {
            if (_dbManager.TableLinksCategories.ContaintByChatId(transmittedData.ChatId) == false)
            {
                return new BotTextMessage(DialogsStringsStorage.MenuShowNoCategories);
            }

            transmittedData.State = State.ClickMenuCategoryDelete;

            ITableLinksCategories tableLinksCategories = _dbManager.TableLinksCategories;

            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuDeleteCategory,
                InlineKeyboardsMarkupStorage.CreateMenuCategoryDelete(linkCategories)
            );
        }
        else if (callBackData == BotButtonsStorage.BackwardInMenuDelete.CallBackData)
        {
            return SharedService.GotoProcessClickInMenuMain(transmittedData);
        }

        throw new Exception(SystemStringsStorage.ErrorWithButtonText);
    }

    public BotTextMessage ProcessClickMenuCategoryDelete(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.BackwardInMenuDelete.CallBackData)
        {
            transmittedData.State = State.ClickInMenuDelete;

            return new BotTextMessage(
                DialogsStringsStorage.MenuDelete,
                InlineKeyboardsMarkupStorage.MenuDelete
            );
        }

        if (!callBackData.StartsWith(SystemStringsStorage.LinkCategoryIdText))
        {
            throw new Exception(SystemStringsStorage.ErrorWithButtonText);
        }

        callBackData = callBackData.Remove(0, SystemStringsStorage.LinkCategoryIdText.Length);

        int categoryId = int.Parse(callBackData);

        ITableLinksCategories tableLinksCategories = _dbManager.TableLinksCategories;

        if (tableLinksCategories.GetAllByChatId(transmittedData.ChatId).Count() == Constants.MinLinksCategoriesCount)
        {
            return new BotTextMessage(DialogsStringsStorage.MenuDeleteCategoryMinOneCategory);
        }

        try
        {
            tableLinksCategories.DeleteById(categoryId);

            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuDeleteCategory,
                InlineKeyboardsMarkupStorage.CreateMenuCategoryDelete(linkCategories)
            );
        }
        catch
        {
            return new BotTextMessage(DialogsStringsStorage.MenuDeleteCategoryUseInLink);
        }
    }

    public BotTextMessage ProcessClickLinkCategoryLinksDelete(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonInChooseCategoryInDeleteLink.CallBackData)
        {
            transmittedData.State = State.ClickInMenuDelete;

            return new BotTextMessage(
                DialogsStringsStorage.MenuDelete,
                InlineKeyboardsMarkupStorage.MenuDelete
            );
        }

        if (!callBackData.StartsWith(SystemStringsStorage.LinkCategoryIdText))
        {
            throw new Exception(SystemStringsStorage.ErrorWithButtonText);
        }

        callBackData = callBackData.Remove(0, SystemStringsStorage.LinkCategoryIdText.Length);

        int categoryId = int.Parse(callBackData);

        transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyLinkCategoryIdForDelete, categoryId);

        IEnumerable<Link> links = _dbManager.TableLinks.GetAllByCategoryId(categoryId);
        

        if (links.Count() != 0)
        {
            transmittedData.State = State.ClickChosenLinkDelete;
            return LinksToText(transmittedData, links);
        }
        else
        {
            transmittedData.State = State.ClickBackwardToChooseCategoryDelete;
            
            return new BotTextMessage(
                DialogsStringsStorage.MenuDeleteNoLinksInCategory,
                InlineKeyboardsMarkupStorage.BackwardToChooseCategoryInDeleteLink
            );
        }
    }

    public BotTextMessage ProcessClickBackwardToChooseCategoryDelete(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonNoLinksInChooseCategoryInDeleteLink.CallBackData)
        {
            if (_dbManager.TableLinksCategories.ContaintByChatId(transmittedData.ChatId) == false)
            {
                return new BotTextMessage(DialogsStringsStorage.MenuDeleteNoCategories);
            }

            transmittedData.State = State.ClickLinkCategoryLinksDelete;

            ITableLinksCategories tableLinksCategories = _dbManager.TableLinksCategories;

            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuDeleteLink,
                InlineKeyboardsMarkupStorage.CreateMenuLinkCategoryLinkDelete(linkCategories)
            );
        }
        throw new Exception(SystemStringsStorage.ErrorWithButtonText);
    }

    public BotTextMessage ProcessClickChosenLinkDelete(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.BackwardInLinkDelete.CallBackData)
        {
            transmittedData.State = State.ClickLinkCategoryLinksDelete;

            ITableLinksCategories tableLinksCategories = _dbManager.TableLinksCategories;

            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuDeleteLink,
                InlineKeyboardsMarkupStorage.CreateMenuLinkCategoryLinkDelete(linkCategories)
            );
        }
        else if (callBackData == BotButtonsStorage.MoreInLinkDelete.CallBackData)
        {
            int startLinkIdMore =
                (int)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyShowLinksStartLinkIdForDelete);

            int categoryIdMore =
                (int)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkCategoryIdForDelete);

            IEnumerable<Link> linksMore = _dbManager.TableLinks
                .GetAllByCategoryIdWithStartLinkId(categoryIdMore, startLinkIdMore);

            return LinksToText(transmittedData, linksMore);
        }

        if (!callBackData.StartsWith(SystemStringsStorage.LinkIdText))
        {
            throw new Exception(SystemStringsStorage.ErrorWithButtonText);
        }

        callBackData = callBackData.Remove(0, SystemStringsStorage.LinkIdText.Length);
        int linkId = int.Parse(callBackData);
        _dbManager.TableLinks.DeleteById(linkId);

        int categoryId =
            (int)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkCategoryIdForDelete);

        IEnumerable<Link> links = _dbManager.TableLinks.GetAllByCategoryId(categoryId);

        return LinksToText(transmittedData, links);
    }
}