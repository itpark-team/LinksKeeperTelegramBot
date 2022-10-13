using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Service.SharedProcessors;
using LinksKeeperTelegramBot.Util.BotButtonsInitializer;
using LinksKeeperTelegramBot.Util.InlineKeyboardsMarkupInitializer;
using LinksKeeperTelegramBot.Util.ReplyTextsInitializer;
using LinksKeeperTelegramBot.Util.Settings;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Service.MainMenu;

public class MainMenuService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    public Task ProcessCommandStart(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        Logger.Info($"Старт метода ProcessCommandStart для chatId = {chatId}");

        string requestMessageText = update.Message.Text;

        string responseMessageText = StringsStorage.Empty;

        if (requestMessageText != StringsStorage.CommandStart)
        {
            responseMessageText = ReplyTextsStorage.CommandStartInputErrorInput;

            Logger.Info($"Команда не распознана. Метод ProcessCommandStart. chatId = {chatId}");

            Task taskError = botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                cancellationToken: cancellationToken);

            Logger.Info($"Отправлено сообщение об ошибке пользователю. Метод ProcessCommandStart. chatId = {chatId}");

            return taskError;
        }

        transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;
        
        responseMessageText = ReplyTextsStorage.MenuMain;

        InlineKeyboardMarkup responseInlineKeyboardMarkup = InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain;
        
        Task taskSuccess = botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            replyMarkup: responseInlineKeyboardMarkup,
            cancellationToken: cancellationToken);

        Logger.Info($"Отправлено корректное сообщение пользователю. Метод ProcessCommandStart. chatId = {chatId}");

        return taskSuccess;
    }

    public Task ProcessClickOnInlineButtonInMenuMain(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        if (update.Message != null && update.Message.Text == StringsStorage.CommandReset)
        {
            return GlobalServices.ProcessCommandReset(chatId, transmittedData, botClient, cancellationToken);
        }

        string requestCallBackData = update.CallbackQuery.Data;
        int messageId = update.CallbackQuery.Message.MessageId;

        string responseMessageText = StringsStorage.Empty;

        if (requestCallBackData == BotButtonsStorage.ButtonAddInMenuMain.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuAdd;
            
            responseMessageText = ReplyTextsStorage.MenuAdd;

            InlineKeyboardMarkup responseInlineKeyboardMarkup =
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuAdd;
            
            return botClient.SendTextMessageAsync(
                chatId: chatId,
                //messageId: messageId,
                text: responseMessageText,
                replyMarkup: responseInlineKeyboardMarkup,
                cancellationToken: cancellationToken
            );
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonShowInMenuMain.CallBackData)
        {
            responseMessageText = "Нажата клавиша Просмотреть";
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonEditInMenuMain.CallBackData)
        {
            responseMessageText = "Нажата клавиша Редактировать";
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonDeleteInMenuMain.CallBackData)
        {
            responseMessageText = "Нажата клавиша Удалить";
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonHowToUseInMenuMain.CallBackData)
        {
            responseMessageText = "Нажата клавиша Как пользоваться";
        }

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            //messageId: messageId,
            text: responseMessageText,
            cancellationToken: cancellationToken
        );
    }

    public Task ProcessClickOnInlineButtonInMenuAddChoosing(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        if (update.Message != null && update.Message.Text == StringsStorage.CommandReset)
        {
            return GlobalServices.ProcessCommandReset(chatId, transmittedData, botClient, cancellationToken);
        }

        string requestCallBackData = update.CallbackQuery.Data;
        int messageId = update.CallbackQuery.Message.MessageId;

        string responseMessageText = StringsStorage.Empty;

        if (requestCallBackData == BotButtonsStorage.ButtonLinkInMenuAdd.CallBackData)
        {
            transmittedData.State = State.WaitingInputLinkUrlForAdd;
            
            responseMessageText = ReplyTextsStorage.LinkInputUrl;
            
            return botClient.SendTextMessageAsync(
                chatId: chatId,
                //messageId: messageId,
                text: responseMessageText,
                cancellationToken: cancellationToken
            );
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonCategoryInMenuAdd.CallBackData)
        {
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonBackwardInMenuAdd.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;
            
            responseMessageText = ReplyTextsStorage.MenuMain;

            InlineKeyboardMarkup responseInlineKeyboardMarkup =
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain;

            return botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
                text: responseMessageText,
                replyMarkup: responseInlineKeyboardMarkup,
                cancellationToken: cancellationToken
            );
        }

        return botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: responseMessageText,
            cancellationToken: cancellationToken
        );
    }
}