using System.Text;
using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
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
}