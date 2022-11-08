using System.Text;
using LinksKeeperTelegramBot.BotInitializer;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;

namespace LinksKeeperTelegramBot.Service.MenuPoints;

public class AddServices
{
    private DbManager _dbManager;

    public AddServices()
    {
        _dbManager = DbManager.GetInstance();
    }

    #region CommonMethods

    private BotTextMessage DisplayCategories(TransmittedData transmittedData)
    {
        IEnumerable<LinkCategory> linkCategories =
            _dbManager.TableLinksCategories.GetAllByChatId(transmittedData.ChatId);

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

            transmittedData.State = State.InputCategoryAdd;
            return new BotTextMessage(replyText);
        }
        else
        {
            replyText += DialogsStringsStorage.MenuCategoryInputRestrict;

            transmittedData.State = State.ClickInMenuCategoryAdd;
            return new BotTextMessage(
                replyText,
                InlineKeyboardsMarkupStorage.MenuCategoryAdd
            );
        }
    }

    #endregion

    public BotTextMessage ProcessClickInMenuAdd(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.LinkInMenuAdd.CallBackData)
        {
            transmittedData.State = State.InputLinkUrlAdd;
            return new BotTextMessage(DialogsStringsStorage.LinkInputUrl);
        }
        else if (callBackData == BotButtonsStorage.CategoryInMenuAdd.CallBackData)
        {
            return DisplayCategories(transmittedData);
        }
        else if (callBackData == BotButtonsStorage.BackwardInMenuAdd.CallBackData)
        {
            return SharedServices.GotoProcessClickInMenuMain(transmittedData);
        }

        throw new Exception("CallBackData не распознана");
    }

    public BotTextMessage ProcessInputLinkUrlAdd(string url, TransmittedData transmittedData)
    {
        if (!url.StartsWith(SystemStringsStorage.Http) && !url.StartsWith(SystemStringsStorage.Https) ||
            !(url.Length >= Constants.MinUrlLength && url.Length <= Constants.MaxUrlLength))
        {
            return new BotTextMessage(DialogsStringsStorage.LinkInputUrlError);
        }

        transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyLinkUrl, url);
        transmittedData.State = State.InputLinkDescriptionAdd;
        return new BotTextMessage(DialogsStringsStorage.LinkInputUrlSuccess);
    }

    public BotTextMessage ProcessInputLinkDescriptionAdd(string description, TransmittedData transmittedData)
    {
        if (!(description.Length >= Constants.MinDescriptionLength &&
              description.Length <= Constants.MaxDescriptionLength))
        {
            return new BotTextMessage(DialogsStringsStorage.LinkInputDescriptionError);
        }

        transmittedData.State = State.ClickLinkCategoryAdd;
        transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyLinkDescription, description);

        TableLinksCategories tableLinksCategories = _dbManager.TableLinksCategories;

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
            InlineKeyboardsMarkupStorage.CreateMenuLinkCategoryAdd(linkCategories)
        );
    }

    public BotTextMessage ProcessClickLinkCategoryAdd(string categoryIdAsString,
        TransmittedData transmittedData)
    {
        if (!categoryIdAsString.StartsWith(SystemStringsStorage.LinkCategoryIdText))
        {
            throw new Exception("LinkCategoryId не распознан");
        }

        categoryIdAsString = categoryIdAsString.Remove(0, SystemStringsStorage.LinkCategoryIdText.Length);

        int categoryId = int.Parse(categoryIdAsString);

        string url = (string)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkUrl);
        string description =
            (string)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkDescription);
        string categoryName = _dbManager.TableLinksCategories.GetById(categoryId).Name;

        Link link = new Link()
        {
            Url = url,
            Description = description,
            CategoryId = categoryId,
            ChatId = transmittedData.ChatId
        };

        transmittedData.DataStorage.AddOrUpdate(SystemStringsStorage.DataStorageKeyLink, link);
        transmittedData.State = State.ClickInMenuApproveAdd;
        return new BotTextMessage(
            DialogsStringsStorage.CreateLinkInputCategory(url, description, categoryName),
            InlineKeyboardsMarkupStorage.MenuApproveAdd
        );
    }

    public BotTextMessage ProcessClickInMenuApproveAdd(string callBackData,
        TransmittedData transmittedData)
    {
        string responseMessageText = SystemStringsStorage.Empty;

        if (callBackData == BotButtonsStorage.YesInMenuApproveAdd.CallBackData)
        {
            Link link = (Link)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLink);

            _dbManager.TableLinks.AddNew(link);

            responseMessageText = DialogsStringsStorage.LinkApproveAddYes;
        }
        else if (callBackData == BotButtonsStorage.NoInMenuApproveAdd.CallBackData)
        {
            responseMessageText = DialogsStringsStorage.LinkApproveAddNo;
        }


        transmittedData.DataStorage.Clear();
        transmittedData.State = State.ClickInMenuAnotherLinkAdd;
        return new BotTextMessage(
            responseMessageText,
            InlineKeyboardsMarkupStorage.AnotherLinkAdd
        );
    }

    public BotTextMessage ProcessClickInMenuAnotherLinkAdd(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.GotoMainMenuInMenuAnotherLinkAdd.CallBackData)
        {
            return SharedServices.GotoProcessClickInMenuMain(transmittedData);
        }
        else if (callBackData == BotButtonsStorage.AddOneInMenuAnotherLinkAdd.CallBackData)
        {
            transmittedData.State = State.InputLinkUrlAdd;
            return new BotTextMessage(DialogsStringsStorage.LinkInputUrl);
        }

        throw new Exception("Bad user request");
    }

    public BotTextMessage ProcessInputCategoryAdd(string categoryName, TransmittedData transmittedData)
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

        _dbManager.TableLinksCategories.AddNew(linkCategory);

        transmittedData.State = State.ClickInMenuAnotherCategoryAdd;
        return new BotTextMessage(
            DialogsStringsStorage.CategoryNameInputSuccess,
            InlineKeyboardsMarkupStorage.MenuAnotherCategoryAdd);
    }

    public BotTextMessage ProcessClickInMenuAnotherCategoryAdd(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.GotoMainMenuInMenuAnotherCategoryAdd.CallBackData)
        {
            return SharedServices.GotoProcessClickInMenuMain(transmittedData);
        }
        else if (callBackData == BotButtonsStorage.AddOneInMenuAnotherCategoryAdd.CallBackData)
        {
            return DisplayCategories(transmittedData);
        }

        throw new Exception("Bad user request");
    }

    public BotTextMessage ProcessClickInMenuCategoryAdd(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.GotoMainMenuInMenuCategoryAdd.CallBackData)
        {
            return SharedServices.GotoProcessClickInMenuMain(transmittedData);
        }

        throw new Exception("Bad user request");
    }
}