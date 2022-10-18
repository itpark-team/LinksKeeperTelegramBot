using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Service.SharedProcessors;
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
        string responseMessageText = StringsStorage.Empty;
        
        string url = requestMessageText;

        if (!url.StartsWith(StringsStorage.Http) && !url.StartsWith(StringsStorage.Https) ||
            !(url.Length >= 10 && url.Length <= 2048))
        {
            responseMessageText = ReplyTextsStorage.LinkInputUrlError;

            return botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                cancellationToken: cancellationToken);
        }

        transmittedData.State = State.WaitingInputLinkDescriptionForAdd;
        transmittedData.DataStorage.Add(StringsStorage.DataStorageKeyLinkUrl, url);

        responseMessageText = ReplyTextsStorage.LinkInputUrlSuccess;

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            cancellationToken: cancellationToken);
    }

    public Task ProcessInputLinkDescriptionForAdd(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        string requestMessageText = update.Message.Text;
        string responseMessageText = StringsStorage.Empty;
        
        string description = requestMessageText;

        if (!(description.Length >= 3 && description.Length <= 256))
        {
            responseMessageText = ReplyTextsStorage.LinkInputDescriptionError;

            return botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                cancellationToken: cancellationToken);
        }

        transmittedData.State = State.WaitingClickOnInlineButtonLinkCategoryForAdd;
        transmittedData.DataStorage.Add(StringsStorage.DataStorageKeyLinkDescription, description);

        responseMessageText = ReplyTextsStorage.LinkInputDescriptionSuccess;

        InlineKeyboardMarkup responseInlineKeyboardMarkup =
            InlineKeyboardsMarkupStorage.CreateInlineKeyboardMarkupMenuLinkCategoryForAdd();

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

        string responseMessageText = StringsStorage.Empty;

        int categoryId = int.Parse(requestCallBackData);

        transmittedData.State = State.WaitingClickOnInlineButtonInMenuApproveAdd;

        transmittedData.DataStorage.Add(StringsStorage.DataStorageKeyLinkCategoryId, categoryId);

        string url = transmittedData.DataStorage.Get(StringsStorage.DataStorageKeyLinkUrl) as string;

        string description = transmittedData.DataStorage.Get(StringsStorage.DataStorageKeyLinkDescription) as string;

        string categoryName = DbManager.GetInstance().TableLinksCategories.getById(categoryId).Name;

        responseMessageText = ReplyTextsStorage.CreateLinkInputCategory(url, description, categoryName);

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

        string responseMessageText = StringsStorage.Empty;

        if (requestCallBackData == BotButtonsStorage.ButtonYesInMenuApproveAdd.CallBackData)
        {
            responseMessageText = ReplyTextsStorage.LinkApproveAddYes;

            string url = transmittedData.DataStorage.Get(StringsStorage.DataStorageKeyLinkUrl) as string;

            string description =
                transmittedData.DataStorage.Get(StringsStorage.DataStorageKeyLinkDescription) as string;

            int categoryId = (int)transmittedData.DataStorage.Get(StringsStorage.DataStorageKeyLinkCategoryId);

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
            responseMessageText = ReplyTextsStorage.LinkApproveAddNo;
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

        string responseMessageText = StringsStorage.Empty;

        if (requestCallBackData == BotButtonsStorage.ButtonGotoMainMenuInMenuAddAnotherLink.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;

            responseMessageText = ReplyTextsStorage.MenuMain;

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
            
            responseMessageText = ReplyTextsStorage.LinkInputUrl;
            
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