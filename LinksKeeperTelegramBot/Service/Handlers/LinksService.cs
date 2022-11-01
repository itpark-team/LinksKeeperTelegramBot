using System.Text;
using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Service.Handlers;

public class LinksService
{
    public BotTextMessage ProcessInputLinkUrlForAdd(string url, TransmittedData transmittedData)
    {
        if (!url.StartsWith(SystemStringsStorage.Http) && !url.StartsWith(SystemStringsStorage.Https) ||
            !(url.Length >= Constants.MinUrlLength && url.Length <= Constants.MaxUrlLength))
        {
            return new BotTextMessage(DialogsStringsStorage.LinkInputUrlError);
        }

        transmittedData.State = State.WaitingInputLinkDescriptionForAdd;
        transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyLinkUrl, url);

        return new BotTextMessage(DialogsStringsStorage.LinkInputUrlSuccess);
    }

    public BotTextMessage ProcessInputLinkDescriptionForAdd(string description, TransmittedData transmittedData)
    {
        if (!(description.Length >= Constants.MinDescriptionLength &&
              description.Length <= Constants.MaxDescriptionLength))
        {
            return new BotTextMessage(DialogsStringsStorage.LinkInputDescriptionError);
        }

        transmittedData.State = State.WaitingClickOnInlineButtonLinkCategoryForAdd;
        transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyLinkDescription, description);

        TableLinksCategories tableLinksCategories = DbManager.GetInstance().TableLinksCategories;

        if (tableLinksCategories.ContaintByChatId(transmittedData.ChatId) == false)
        {
            tableLinksCategories.AddNew(new LinkCategory()
            {
                Name = SystemStringsStorage.FirstLinkCategory,
                ChatId = transmittedData.ChatId
            });
        }

        IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllByChatId(transmittedData.ChatId);

        return new BotTextMessage(
            DialogsStringsStorage.LinkInputDescriptionSuccess,
            InlineKeyboardsMarkupStorage.CreateInlineKeyboardMarkupMenuLinkCategoryForAdd(linkCategories)
        );
    }

    public BotTextMessage ProcessClickOnInlineButtonLinkCategoryForAdd(string categoryIdAsString,
        TransmittedData transmittedData)
    {
        if (!categoryIdAsString.StartsWith(SystemStringsStorage.LinkCategoryIdText))
        {
            throw new Exception("Bad user request");
        }

        categoryIdAsString = categoryIdAsString.Remove(0, SystemStringsStorage.LinkCategoryIdText.Length);

        int categoryId = int.Parse(categoryIdAsString);

        transmittedData.State = State.WaitingClickOnInlineButtonInMenuApproveAdd;
        transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyLinkCategoryId, categoryId);

        string url = transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkUrl) as string;
        string description =
            transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkDescription) as string;
        string categoryName = DbManager.GetInstance().TableLinksCategories.GetById(categoryId).Name;

        return new BotTextMessage(
            DialogsStringsStorage.CreateLinkInputCategory(url, description, categoryName),
            InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuApproveAdd
        );
    }

    public BotTextMessage ProcessClickOnInlineButtonInMenuApproveAdd(string callBackData,
        TransmittedData transmittedData)
    {
        string responseMessageText = SystemStringsStorage.Empty;

        if (callBackData == BotButtonsStorage.ButtonYesInMenuApproveAdd.CallBackData)
        {
            string url = transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkUrl) as string;

            string description =
                transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkDescription) as string;

            int categoryId = (int)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkCategoryId);

            Link link = new Link()
            {
                Url = url,
                Description = description,
                CategoryId = categoryId,
                ChatId = transmittedData.ChatId
            };

            DbManager.GetInstance().TableLinks.AddNew(link);

            responseMessageText = DialogsStringsStorage.LinkApproveAddYes;
        }
        else if (callBackData == BotButtonsStorage.ButtonNoInMenuApproveAdd.CallBackData)
        {
            responseMessageText = DialogsStringsStorage.LinkApproveAddNo;
        }

        transmittedData.State = State.WaitingClickOnInlineButtonInMenuAddAnotherLink;
        transmittedData.DataStorage.Clear();

        return new BotTextMessage(
            responseMessageText,
            InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuAddAnotherLink
        );
    }

    public BotTextMessage ProcessClickOnInlineButtonInMenuAddAnotherLink(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonGotoMainMenuInMenuAddAnotherLink.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;

            return new BotTextMessage(
                DialogsStringsStorage.MenuMain,
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain
            );
        }
        else if (callBackData == BotButtonsStorage.ButtonAddOneInMenuAddAnotherLink.CallBackData)
        {
            transmittedData.State = State.WaitingInputLinkUrlForAdd;

            return new BotTextMessage(DialogsStringsStorage.LinkInputUrl);
        }

        throw new Exception("Bad user request");
    }

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
            int currentLinkTextLength = link.Url.Length + link.Description.Length + SystemStringsStorage.Devider.Length;

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
            stringBuilder.AppendLine("-----");
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
                    link.Url.Length + link.Description.Length + SystemStringsStorage.Devider.Length;

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
                stringBuilder.AppendLine("-----");
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

        return new BotTextMessage("aaa");
    }
}