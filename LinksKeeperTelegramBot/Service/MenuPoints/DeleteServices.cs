using System.Text;
using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Service.MenuPoints;

public class DeleteServices
{
     public BotTextMessage ProcessClickInMenuDelete(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.LinkInMenuDelete.CallBackData)
        {
            if (DbManager.GetInstance().TableLinksCategories.ContaintByChatId(transmittedData.ChatId) == false)
            {
                return new BotTextMessage(DialogsStringsStorage.MenuDeleteNoCategories);
            }

            transmittedData.State = State.ClickLinkCategoryLinksDelete;

            TableLinksCategories tableLinksCategories = DbManager.GetInstance().TableLinksCategories;

            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuDeleteLink,
                InlineKeyboardsMarkupStorage.CreateMenuLinkCategoryLinkDelete(linkCategories)
            );
            
        }
        else if (callBackData == BotButtonsStorage.CategoryInMenuDelete.CallBackData)
        {
            if (DbManager.GetInstance().TableLinksCategories.ContaintByChatId(transmittedData.ChatId) == false)
            {
                return new BotTextMessage(DialogsStringsStorage.MenuShowNoCategories);
            }

            transmittedData.State = State.ClickMenuCategoryDelete;

            TableLinksCategories tableLinksCategories = DbManager.GetInstance().TableLinksCategories;

            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuDeleteCategory,
                InlineKeyboardsMarkupStorage.CreateMenuCategoryDelete(linkCategories)
            );
        }
        else if (callBackData == BotButtonsStorage.BackwardInMenuDelete.CallBackData)
        {
            return SharedServices.GotoProcessClickInMenuMain(transmittedData);
        }

        throw new Exception("Bad user request");
    }
     
      public BotTextMessage ProcessClickClickLinkCategoryLinksDelete(string callBackData,
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
            throw new Exception("Bad user request");
        }

        callBackData = callBackData.Remove(0, SystemStringsStorage.LinkCategoryIdText.Length);

        int categoryId = int.Parse(callBackData);

        transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyLinkCategoryIdForDelete, categoryId);

        IEnumerable<Link> links = DbManager.GetInstance().TableLinks.GetAllByCategoryId(categoryId);

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
                transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyShowLinksStartLinkIdForDelete,
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
        InlineKeyboardMarkup inlineKeyboardMarkup = InlineKeyboardMarkup.Empty();

        transmittedData.State = State.ClickChosenLinkDelete;

        if (hasMessageFit)
        {
            inlineKeyboardMarkup = InlineKeyboardsMarkupStorage.CreateLinksAllDelete(fitLinks);
        }
        else
        {
            inlineKeyboardMarkup = InlineKeyboardsMarkupStorage.CreateLinksAllMoreDelete(fitLinks);
        }

        return new BotTextMessage(
            linksAsText,
            inlineKeyboardMarkup
        );

    }

    public BotTextMessage ProcessClickChosenLinkDelete(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.BackwardInLinkDelete.CallBackData)
        {
            transmittedData.State = State.ClickLinkCategoryLinksDelete;

            TableLinksCategories tableLinksCategories = DbManager.GetInstance().TableLinksCategories;

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

            IEnumerable<Link> linksMore = DbManager.GetInstance().TableLinks
                .GetAllByCategoryIdWithStartLinkId(categoryIdMore, startLinkIdMore);
            
            StringBuilder stringBuilderMore = new StringBuilder();

            bool hasMessageFitMore = true;

            List<Link> fitLinksMore = new List<Link>();
            
            foreach (Link link in linksMore)
            {
                int stringBuilderTextLength = stringBuilderMore.Length;
                int currentLinkTextLength = SystemStringsStorage.LinkId.Length + link.Id.ToString().Length +
                                            link.Url.Length + link.Description.Length +
                                            SystemStringsStorage.Devider.Length + Constants.EnterSymbolsLength;

                if (stringBuilderTextLength + currentLinkTextLength > Constants.MaxBotTextMessageLength)
                {
                    hasMessageFitMore = false;
                    transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyShowLinksStartLinkIdForDelete,
                        link.Id);
                    break;
                }

                stringBuilderMore.AppendLine(SystemStringsStorage.LinkId + link.Id);
                stringBuilderMore.AppendLine(link.Url);
                stringBuilderMore.AppendLine(link.Description);
                stringBuilderMore.AppendLine(SystemStringsStorage.Devider);

                fitLinksMore.Add(link);
            }
            
            string linksAsTextMore = stringBuilderMore.ToString();
            InlineKeyboardMarkup inlineKeyboardMarkupMore = InlineKeyboardMarkup.Empty();
            
            if (hasMessageFitMore)
            {
                inlineKeyboardMarkupMore = InlineKeyboardsMarkupStorage.CreateLinksAllDelete(fitLinksMore);
            }
            else
            {
                inlineKeyboardMarkupMore = InlineKeyboardsMarkupStorage.CreateLinksAllMoreDelete(fitLinksMore);
            }

            return new BotTextMessage(
                linksAsTextMore,
                inlineKeyboardMarkupMore
            );
        }

        if (!callBackData.StartsWith(SystemStringsStorage.LinkIdText))
        {
            throw new Exception("Bad user request");
        }

        callBackData = callBackData.Remove(0, SystemStringsStorage.LinkIdText.Length);
        int linkId = int.Parse(callBackData);
        DbManager.GetInstance().TableLinks.DeleteById(linkId);

        int categoryId = 
            (int)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkCategoryIdForDelete);

        IEnumerable<Link> links = DbManager.GetInstance().TableLinks.GetAllByCategoryId(categoryId);

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
                transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyShowLinksStartLinkIdForDelete,
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
        InlineKeyboardMarkup inlineKeyboardMarkup = InlineKeyboardMarkup.Empty();

        if (hasMessageFit)
        {
            inlineKeyboardMarkup = InlineKeyboardsMarkupStorage.CreateLinksAllDelete(fitLinks);
        }
        else
        {
            inlineKeyboardMarkup = InlineKeyboardsMarkupStorage.CreateLinksAllMoreDelete(fitLinks);
        }
        
        return new BotTextMessage(
            linksAsText,
            inlineKeyboardMarkup
        );
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
            throw new Exception("Bad user request");
        }

        callBackData = callBackData.Remove(0, SystemStringsStorage.LinkCategoryIdText.Length);

        int categoryId = int.Parse(callBackData);

        TableLinksCategories tableLinksCategories = DbManager.GetInstance().TableLinksCategories;

        if (tableLinksCategories.GetAllByChatId(transmittedData.ChatId).Count() == Constants.MinLinksCategoriesCount)
        {
            return new BotTextMessage("Нельзя удалить категорию. Т.к. у Вас должна остаться минимум одна категория");
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
        catch (Exception e)
        {
            return new BotTextMessage("Ошибка при удалении категории. Категория используется в существующей ссылке");
        }
    }
}