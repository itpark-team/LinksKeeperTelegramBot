using System.Text;
using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Service.MenuPoints;

public class ShowServices
{
    #region CommonMethods

    private void LinksToText(TransmittedData transmittedData, IEnumerable<Link> links, out string linksAsText,
        out bool hasMessageFit)
    {
        StringBuilder stringBuilder = new StringBuilder();

        hasMessageFit = true;

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

        linksAsText = stringBuilder.ToString();
    }

    #endregion

    public BotTextMessage ProcessClickLinkCategoryShow(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.BackwardInMenuShow.CallBackData)
        {
            return SharedServices.GotoProcessClickInMenuMain(transmittedData);
        }

        if (!callBackData.StartsWith(SystemStringsStorage.LinkCategoryIdText))
        {
            throw new Exception("Bad user request");
        }
        
        callBackData = callBackData.Remove(0, SystemStringsStorage.LinkCategoryIdText.Length);

        int categoryId = int.Parse(callBackData);

        IEnumerable<Link> links = DbManager.GetInstance().TableLinks.GetAllByCategoryId(categoryId);

        LinksToText(transmittedData, links, out string linksAsText, out bool hasMessageFit);

        InlineKeyboardMarkup inlineKeyboardMarkup = hasMessageFit
            ? InlineKeyboardsMarkupStorage.LinksAllShow
            : InlineKeyboardsMarkupStorage.LinksMoreShow;

        transmittedData.State = State.ClickLinkCategoryLinksShow;
        return new BotTextMessage(
            linksAsText,
            inlineKeyboardMarkup
        );
    }

    public BotTextMessage ProcessClickLinkCategoryLinksShow(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.BackwardInLinksShow.CallBackData)
        {
            transmittedData.DataStorage.Delete(SystemStringsStorage.DataStorageKeyShowLinksStartLinkId);
            transmittedData.DataStorage.Delete(SystemStringsStorage.DataStorageKeyShowLinksCategoryId);

            transmittedData.State = State.ClickLinkCategoryShow;

            TableLinksCategories tableLinksCategories = DbManager.GetInstance().TableLinksCategories;

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

            IEnumerable<Link> links = DbManager.GetInstance().TableLinks
                .GetAllByCategoryIdWithStartLinkId(categoryId, startLinkId);

            LinksToText(transmittedData, links, out string linksAsText, out bool hasMessageFit);
          
            InlineKeyboardMarkup inlineKeyboardMarkup = hasMessageFit
                ? InlineKeyboardsMarkupStorage.LinksAllShow
                : InlineKeyboardsMarkupStorage.LinksMoreShow;

            return new BotTextMessage(
                linksAsText,
                inlineKeyboardMarkup
            );
        }

        throw new Exception("Bad user request");
    }
}