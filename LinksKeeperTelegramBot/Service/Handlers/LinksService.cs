using System.Text;
using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
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
            !(url.Length >= 10 && url.Length <= 2048))
        {
            return new BotTextMessage(DialogsStringsStorage.LinkInputUrlError);
        }

        transmittedData.State = State.WaitingInputLinkDescriptionForAdd;
        transmittedData.DataStorage.Add(SystemStringsStorage.DataStorageKeyLinkUrl, url);

        return new BotTextMessage(DialogsStringsStorage.LinkInputUrlSuccess);
    }

    public BotTextMessage ProcessInputLinkDescriptionForAdd(string description, TransmittedData transmittedData)
    {
        if (!(description.Length >= 3 && description.Length <= 256))
        {
            return new BotTextMessage(DialogsStringsStorage.LinkInputDescriptionError);
        }

        transmittedData.State = State.WaitingClickOnInlineButtonLinkCategoryForAdd;
        transmittedData.DataStorage.Add(SystemStringsStorage.DataStorageKeyLinkDescription, description);

        return new BotTextMessage(
            DialogsStringsStorage.LinkInputDescriptionSuccess,
            InlineKeyboardsMarkupStorage.CreateInlineKeyboardMarkupMenuLinkCategoryForAdd(transmittedData.ChatId)
        );
    }

    public BotTextMessage ProcessClickOnInlineButtonLinkCategoryForAdd(string categoryIdAsString,
        TransmittedData transmittedData)
    {
        int categoryId = int.Parse(categoryIdAsString);

        transmittedData.State = State.WaitingClickOnInlineButtonInMenuApproveAdd;
        transmittedData.DataStorage.Add(SystemStringsStorage.DataStorageKeyLinkCategoryId, categoryId);

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

        return new BotTextMessage();
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

        int categoryId = int.Parse(callBackData);

        IEnumerable<Link> links = DbManager.GetInstance().TableLinks.GetAllByCategoryId(categoryId);

        StringBuilder stringBuilder = new StringBuilder();
        foreach (Link link in links)
        {
            stringBuilder.AppendLine(link.Url);
            stringBuilder.AppendLine(link.Description);
            stringBuilder.AppendLine("-----");
        }
        //todo button ЕЩЁ

        return new BotTextMessage(stringBuilder.ToString());
    }
}