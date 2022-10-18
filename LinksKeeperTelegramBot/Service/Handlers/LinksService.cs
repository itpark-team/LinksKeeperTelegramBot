using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util.BotButtonsInitializer;
using LinksKeeperTelegramBot.Util.InlineKeyboardsMarkupInitializer;
using LinksKeeperTelegramBot.Util.ReplyTextsInitializer;
using LinksKeeperTelegramBot.Util.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Service.Links;

public class LinksService
{
    public Task ProcessInputLinkUrlForAdd(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        string requestMessageText = update.Message.Text;
        string responseMessageText = SystemStringsStorage.Empty;
        
        string url = requestMessageText;

        if (!url.StartsWith(SystemStringsStorage.Http) && !url.StartsWith(SystemStringsStorage.Https) ||
            !(url.Length >= 10 && url.Length <= 2048))
        {
            responseMessageText = DialogsStringsStorage.LinkInputUrlError;

            return botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                cancellationToken: cancellationToken);
        }

        transmittedData.State = State.WaitingInputLinkDescriptionForAdd;
        transmittedData.DataStorage.Add(SystemStringsStorage.DataStorageKeyLinkUrl, url);

        responseMessageText = DialogsStringsStorage.LinkInputUrlSuccess;

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            cancellationToken: cancellationToken);
    }

    public Task ProcessInputLinkDescriptionForAdd(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        string requestMessageText = update.Message.Text;
        string responseMessageText = SystemStringsStorage.Empty;
        
        string description = requestMessageText;

        if (!(description.Length >= 3 && description.Length <= 256))
        {
            responseMessageText = DialogsStringsStorage.LinkInputDescriptionError;

            return botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                cancellationToken: cancellationToken);
        }

        transmittedData.State = State.WaitingClickOnInlineButtonLinkCategoryForAdd;
        transmittedData.DataStorage.Add(SystemStringsStorage.DataStorageKeyLinkDescription, description);

        responseMessageText = DialogsStringsStorage.LinkInputDescriptionSuccess;

        InlineKeyboardMarkup responseInlineKeyboardMarkup =
            InlineKeyboardsMarkupStorage.CreateInlineKeyboardMarkupMenuLinkCategoryForAdd(chatId);

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            replyMarkup: responseInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    public Task ProcessClickOnInlineButtonLinkCategoryForAdd(long chatId, TransmittedData transmittedData,
        Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        string requestCallBackData = update.CallbackQuery.Data;

        string responseMessageText = SystemStringsStorage.Empty;

        int categoryId = int.Parse(requestCallBackData);

        transmittedData.State = State.WaitingClickOnInlineButtonInMenuApproveAdd;
        transmittedData.DataStorage.Add(SystemStringsStorage.DataStorageKeyLinkCategoryId, categoryId);

        string url = transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkUrl) as string;
        string description = transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkDescription) as string;

        string categoryName = DbManager.GetInstance().TableLinksCategories.getById(categoryId).Name;

        responseMessageText = DialogsStringsStorage.CreateLinkInputCategory(url, description, categoryName);

        InlineKeyboardMarkup responseInlineKeyboardMarkup =
            InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuApproveAdd;

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            replyMarkup: responseInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    public Task ProcessClickOnInlineButtonInMenuApproveAdd(long chatId, TransmittedData transmittedData,
        Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        string requestCallBackData = update.CallbackQuery.Data;

        string responseMessageText = SystemStringsStorage.Empty;

        if (requestCallBackData == BotButtonsStorage.ButtonYesInMenuApproveAdd.CallBackData)
        {
            responseMessageText = DialogsStringsStorage.LinkApproveAddYes;

            string url = transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkUrl) as string;

            string description =
                transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkDescription) as string;

            int categoryId = (int)transmittedData.DataStorage.Get(SystemStringsStorage.DataStorageKeyLinkCategoryId);

            Link link = new Link()
            {
                Url = url,
                Description = description,
                CategoryId = categoryId,
                ChatId = chatId
            };

            DbManager.GetInstance().TableLinks.addNew(link);
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonNoInMenuApproveAdd.CallBackData)
        {
            responseMessageText = DialogsStringsStorage.LinkApproveAddNo;
        }

        transmittedData.State = State.WaitingClickOnInlineButtonInMenuAddAnotherLink;
        transmittedData.DataStorage.Clear();

        InlineKeyboardMarkup responseInlineKeyboardMarkup =
            InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuAddAnotherLink;

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            replyMarkup: responseInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }

    public Task ProcessClickOnInlineButtonInMenuAddAnotherLink(long chatId, TransmittedData transmittedData,
        Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        string requestCallBackData = update.CallbackQuery.Data;

        string responseMessageText = SystemStringsStorage.Empty;

        if (requestCallBackData == BotButtonsStorage.ButtonGotoMainMenuInMenuAddAnotherLink.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;

            responseMessageText = DialogsStringsStorage.MenuMain;

            InlineKeyboardMarkup responseInlineKeyboardMarkup =
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain;

            return botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                replyMarkup: responseInlineKeyboardMarkup,
                cancellationToken: cancellationToken);
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonAddOneInMenuAddAnotherLink.CallBackData)
        {
            transmittedData.State = State.WaitingInputLinkUrlForAdd;
            
            responseMessageText = DialogsStringsStorage.LinkInputUrl;
            
            return botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                cancellationToken: cancellationToken
            );
        }
        
        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            cancellationToken: cancellationToken
        );
    }
}