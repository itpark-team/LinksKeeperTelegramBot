using System.Text;
using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Service.Services;

public class MenuPointShowServices
{
    public BotTextMessage ProcessClickOnInlineButtonLinkCategoryForShow(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonBackwardInMenuShow.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;

            return new BotTextMessage(
                DialogsStringsStorage.MenuMain,
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain
            );
        }

        if (!callBackData.StartsWith(SystemStringsStorage.LinkCategoryIdText))
        {
            throw new Exception("Bad user request");
        }

        callBackData = callBackData.Remove(0, SystemStringsStorage.LinkCategoryIdText.Length);

        int categoryId = int.Parse(callBackData);

        IEnumerable<Link> links = DbManager.GetInstance().TableLinks.GetAllByCategoryId(categoryId);

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
        InlineKeyboardMarkup inlineKeyboardMarkup = InlineKeyboardMarkup.Empty();

        transmittedData.State = State.WaitingClickOnInlineButtonLinkCategoryShowLinks;

        if (hasMessageFit)
        {
            inlineKeyboardMarkup = InlineKeyboardsMarkupStorage.InlineKeyboardMarkupShowLinksAll;
        }
        else
        {
            inlineKeyboardMarkup = InlineKeyboardsMarkupStorage.InlineKeyboardMarkupShowLinksMore;
        }

        return new BotTextMessage(
            linksAsText,
            inlineKeyboardMarkup
        );
    }

    public BotTextMessage ProcessClickOnInlineButtonLinkCategoryShowLinks(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonBackwardInShowLinks.CallBackData)
        {
            transmittedData.DataStorage.Delete(SystemStringsStorage.DataStorageKeyShowLinksStartLinkId);
            transmittedData.DataStorage.Delete(SystemStringsStorage.DataStorageKeyShowLinksCategoryId);

            transmittedData.State = State.WaitingClickOnInlineButtonLinkCategoryForShow;

            TableLinksCategories tableLinksCategories = DbManager.GetInstance().TableLinksCategories;

            IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

            return new BotTextMessage(
                DialogsStringsStorage.MenuShow,
                InlineKeyboardsMarkupStorage.CreateInlineKeyboardMarkupMenuLinkCategoryForShow(linkCategories)
            );
        }
        else if (callBackData == BotButtonsStorage.ButtonMoreInShowLinks.CallBackData)
        {
            int categoryId =
                (int)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyShowLinksCategoryId);
            int startLinkId =
                (int)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyShowLinksStartLinkId);

            IEnumerable<Link> links = DbManager.GetInstance().TableLinks
                .GetAllByCategoryIdWithStartLinkId(categoryId, startLinkId);

            StringBuilder stringBuilder = new StringBuilder();

            bool hasMessageFit = true;

            foreach (Link link in links)
            {
                int stringBuilderTextLength = stringBuilder.Length;
                int currentLinkTextLength =
                    link.Url.Length + link.Description.Length + SystemStringsStorage.Devider.Length +
                    Constants.EnterSymbolsLength;

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
            InlineKeyboardMarkup inlineKeyboardMarkup = InlineKeyboardMarkup.Empty();

            if (hasMessageFit)
            {
                transmittedData.DataStorage.Delete(SystemStringsStorage.DataStorageKeyShowLinksStartLinkId);
                transmittedData.DataStorage.Delete(SystemStringsStorage.DataStorageKeyShowLinksCategoryId);

                inlineKeyboardMarkup = InlineKeyboardsMarkupStorage.InlineKeyboardMarkupShowLinksAll;
            }
            else
            {
                inlineKeyboardMarkup = InlineKeyboardsMarkupStorage.InlineKeyboardMarkupShowLinksMore;
            }

            return new BotTextMessage(
                linksAsText,
                inlineKeyboardMarkup
            );
        }

        throw new Exception("Bad user request");
    }
}