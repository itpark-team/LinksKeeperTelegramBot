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
    
     public BotTextMessage ProcessInputCategoryForAdd(string categoryName, TransmittedData transmittedData)
    {
        if (!(categoryName.Length >= Constants.MinCategoryNameLength &&
              categoryName.Length <= Constants.MaxCategoryNameLength))
        {
            return new BotTextMessage(DialogsStringsStorage.CategoryNameInputError);
        }

        transmittedData.State = State.WaitingClickOnInlineButtonInMenuAddAnotherCategory;

        LinkCategory linkCategory = new LinkCategory()
        {
            Name = categoryName,
            ChatId = transmittedData.ChatId
        };

        DbManager.GetInstance().TableLinksCategories.AddNew(linkCategory);

        return new BotTextMessage(
            DialogsStringsStorage.CategoryNameInputSuccess,
            InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuAddAnotherCategory);
    }
     
    public BotTextMessage ProcessClickOnInlineButtonInMenuAddAnotherCategory(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonGotoMainMenuInMenuAddAnotherCategory.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;

            return new BotTextMessage(
                DialogsStringsStorage.MenuMain,
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain
            );
        }
        else if (callBackData == BotButtonsStorage.ButtonAddOneInMenuAddAnotherCategory.CallBackData)
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

        throw new Exception("Bad user request");
    }

    public BotTextMessage ProcessClickOnInlineButtonInMenuAddCategory(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonGotoMainMenuInMenuAddCategory.CallBackData)
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