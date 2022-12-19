using System.Text;
using LinksKeeperTelegramBot.BotInitializer;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Service.MenuPoints;

public class ShowService
{
    private DbManager _dbManager;

    public ShowService(DbManager dbManager)
    {
        _dbManager = dbManager;
    }

    #region CommonMethods

    private BotTextMessage LinksToText(TransmittedData transmittedData, IEnumerable<Link> links)
    {
        StringBuilder stringBuilder = new StringBuilder();

        bool hasMessageFit = true;

        foreach (Link link in links)
        {
            int stringBuilderTextLength = stringBuilder.Length;
            int currentLinkTextLength = link.Url.Length + link.Description.Length +
                                        SystemStringsStorage.Devider.Length + Constants.EnterSymbolsLength;

            if (stringBuilderTextLength + currentLinkTextLength > Constants.MaxBotTextMessageLength)
            {
                hasMessageFit = false;
                transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyShowLinksStartLinkId,
                    link.Id);
                transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyShowLinksCategoryId,
                    link.CategoryId);
                break;
            }

            stringBuilder.AppendLine(link.Url);
            stringBuilder.AppendLine(link.Description);
            stringBuilder.AppendLine(SystemStringsStorage.Devider);
        }

        string linksAsText = stringBuilder.ToString();
        
        InlineKeyboardMarkup inlineKeyboardMarkup = hasMessageFit
            ? InlineKeyboardsMarkupStorage.LinksAllShow
            : InlineKeyboardsMarkupStorage.LinksMoreShow;
        
        return new BotTextMessage(
            linksAsText,
            inlineKeyboardMarkup
        );
    }

    #endregion

    public BotTextMessage ProcessClickLinkCategoryShow(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.BackwardInMenuShow.CallBackData)
        {
            return SharedService.GotoProcessClickInMenuMain(transmittedData);
        }

        if (!callBackData.StartsWith(SystemStringsStorage.LinkCategoryIdText))
        {
            throw new Exception(SystemStringsStorage.ErrorWithLinkCategoryIdText);
        }
        
        callBackData = callBackData.Remove(0, SystemStringsStorage.LinkCategoryIdText.Length);

        int categoryId = int.Parse(callBackData);

        IEnumerable<Link> links = _dbManager.TableLinks.GetAllByCategoryId(categoryId);

        if (links.Count() != 0)
        {
            transmittedData.State = State.ClickLinkCategoryLinksShow;
            return LinksToText(transmittedData, links);
        }
        else
        {
            transmittedData.State = State.ClickBackwardCategoryLinksShow;
            return new BotTextMessage(
                DialogsStringsStorage.MenuShowNoLinksInCategory,
                InlineKeyboardsMarkupStorage.BackwardToChooseCategoryInLinksShow
            );
        }
    }

    public BotTextMessage ProcessClickBackwardCategoryLinksShow(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.BackwardToChooseCategoryInLinksShow.CallBackData)
        {
            ITableLinksCategories tableLinksCategories = _dbManager.TableLinksCategories;
            
            if (tableLinksCategories.ContaintByChatId(transmittedData.ChatId) == false)
            {
                return new BotTextMessage(DialogsStringsStorage.MenuShowNoCategories);
            }

            transmittedData.State = State.ClickLinkCategoryShow;
            
            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuShow,
                InlineKeyboardsMarkupStorage.CreateMenuLinkCategoryShow(linkCategories)
            );
        }
        throw new Exception(SystemStringsStorage.ErrorWithButtonText);
    }

    public BotTextMessage ProcessClickLinkCategoryLinksShow(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.BackwardInLinksShow.CallBackData)
        {
            transmittedData.DataStorage.Delete(SystemStringsStorage.DataStorageKeyShowLinksStartLinkId);
            transmittedData.DataStorage.Delete(SystemStringsStorage.DataStorageKeyShowLinksCategoryId);

            transmittedData.State = State.ClickLinkCategoryShow;

            ITableLinksCategories tableLinksCategories = _dbManager.TableLinksCategories;

            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuShow,
                InlineKeyboardsMarkupStorage.CreateMenuLinkCategoryShow(linkCategories)
            );
        }
        else if (callBackData == BotButtonsStorage.MoreInLinksShow.CallBackData)
        {
            int categoryId =
                (int)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyShowLinksCategoryId);
            int startLinkId =
                (int)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyShowLinksStartLinkId);

            IEnumerable<Link> links = _dbManager.TableLinks
                .GetAllByCategoryIdWithStartLinkId(categoryId, startLinkId);

            return LinksToText(transmittedData, links);
        }

        throw new Exception(SystemStringsStorage.ErrorWithButtonText);
    }
}