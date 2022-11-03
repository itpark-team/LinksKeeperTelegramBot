using System.Text;
using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;

namespace LinksKeeperTelegramBot.Service.Services;

public class MenuPointAddServices
{
    #region SharedMethods
    private BotTextMessage DisplayCategories(TransmittedData transmittedData)
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

    #endregion

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
            return DisplayCategories(transmittedData);
        }
        else if (callBackData == BotButtonsStorage.ButtonBackwardInMenuAdd.CallBackData)
        {
            return SharedServices.GotoProcessClickOnInlineButtonInMenuMain(transmittedData);
        }

        throw new Exception("Bad user request");
    }

    public BotTextMessage ProcessInputLinkUrlForAdd(string url, TransmittedData transmittedData)
    {
        if (!url.StartsWith(SystemStringsStorage.Http) && !url.StartsWith(SystemStringsStorage.Https) ||
            !(url.Length >= Constants.MinUrlLength && url.Length <= Constants.MaxUrlLength))
        {
            return new BotTextMessage(DialogsStringsStorage.LinkInputUrlError);
        }
        
        transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyLinkUrl, url);
        transmittedData.State = State.WaitingInputLinkDescriptionForAdd;
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
        
        string url = (string)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkUrl);
        string description =
            (string)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkDescription);
        string categoryName = DbManager.GetInstance().TableLinksCategories.GetById(categoryId).Name;

        Link link = new Link()
        {
            Url = url,
            Description = description,
            CategoryId = categoryId,
            ChatId = transmittedData.ChatId
        };

        transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyLink, link);
        transmittedData.State = State.WaitingClickOnInlineButtonInMenuApproveAdd;
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
            Link link = (Link)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLink);

            DbManager.GetInstance().TableLinks.AddNew(link);

            responseMessageText = DialogsStringsStorage.LinkApproveAddYes;
        }
        else if (callBackData == BotButtonsStorage.ButtonNoInMenuApproveAdd.CallBackData)
        {
            responseMessageText = DialogsStringsStorage.LinkApproveAddNo;
        }

        
        transmittedData.DataStorage.Clear();
        transmittedData.State = State.WaitingClickOnInlineButtonInMenuAddAnotherLink;
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
            return SharedServices.GotoProcessClickOnInlineButtonInMenuMain(transmittedData);
        }
        else if (callBackData == BotButtonsStorage.ButtonAddOneInMenuAddAnotherLink.CallBackData)
        {
            transmittedData.State = State.WaitingInputLinkUrlForAdd;
            return new BotTextMessage(DialogsStringsStorage.LinkInputUrl);
        }

        throw new Exception("Bad user request");
    }

    public BotTextMessage ProcessInputCategoryForAdd(string categoryName, TransmittedData transmittedData)
    {
        if (!(categoryName.Length >= Constants.MinCategoryNameLength &&
              categoryName.Length <= Constants.MaxCategoryNameLength))
        {
            return new BotTextMessage(DialogsStringsStorage.CategoryNameInputError);
        }

        LinkCategory linkCategory = new LinkCategory()
        {
            Name = categoryName,
            ChatId = transmittedData.ChatId
        };

        DbManager.GetInstance().TableLinksCategories.AddNew(linkCategory);
        
        transmittedData.State = State.WaitingClickOnInlineButtonInMenuAddAnotherCategory;
        return new BotTextMessage(
            DialogsStringsStorage.CategoryNameInputSuccess,
            InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuAddAnotherCategory);
    }

    public BotTextMessage ProcessClickOnInlineButtonInMenuAddAnotherCategory(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonGotoMainMenuInMenuAddAnotherCategory.CallBackData)
        {
            return SharedServices.GotoProcessClickOnInlineButtonInMenuMain(transmittedData);
        }
        else if (callBackData == BotButtonsStorage.ButtonAddOneInMenuAddAnotherCategory.CallBackData)
        {
            return DisplayCategories(transmittedData);
        }

        throw new Exception("Bad user request");
    }

    public BotTextMessage ProcessClickOnInlineButtonInMenuAddCategory(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonGotoMainMenuInMenuAddCategory.CallBackData)
        {
            return SharedServices.GotoProcessClickOnInlineButtonInMenuMain(transmittedData);
        }

        throw new Exception("Bad user request");
    }
}