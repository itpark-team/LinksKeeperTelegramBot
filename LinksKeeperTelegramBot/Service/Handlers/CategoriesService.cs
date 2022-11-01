using System.Text;
using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;

namespace LinksKeeperTelegramBot.Service.Handlers;

public class CategoriesService
{
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


    public BotTextMessage ProcessClickOnInlineButtonInMenuDeleteCategory(string callBackData,
        TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonBackwardInMenuDelete.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuDelete;

            return new BotTextMessage(
                DialogsStringsStorage.MenuDelete,
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuDelete
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
                InlineKeyboardsMarkupStorage.CreateInlineKeyboardMarkupMenuDeleteCategory(linkCategories)
            );
        }
        catch (Exception e)
        {
            return new BotTextMessage("Ошибка при удалении категории. Категория используется в существующей ссылке");
        }
    }
}