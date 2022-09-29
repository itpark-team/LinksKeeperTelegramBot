using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util.BotButtonsInitializer;
using LinksKeeperTelegramBot.Util.InlineKeyboardsMarkupInitializer;
using LinksKeeperTelegramBot.Util.ReplyTextsInitializer;
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

        string responseMessageText = "empty";

        if (requestMessageText != "/start")
        {
            responseMessageText = "Команда не распознана. Для начала работы с ботом введите /start";

            Logger.Info($"Команда не распознана. Метод ProcessCommandStart. chatId = {chatId}");

            Task taskError = botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                cancellationToken: cancellationToken);

            Logger.Info($"Отправлено сообщение об ошибке пользователю. Метод ProcessCommandStart. chatId = {chatId}");

            return taskError;
        }

        responseMessageText = ReplyTextsStorage.MenuMain;

        InlineKeyboardMarkup responseInlineKeyboardMarkup = InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain;

        transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;

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
        string requestCallBackData = update.CallbackQuery.Data;
        int messageId = update.CallbackQuery.Message.MessageId;

        string responseMessageText = "empty";

        if (requestCallBackData == BotButtonsStorage.ButtonAddInMenuMain.CallBackData)
        {
            responseMessageText = ReplyTextsStorage.MenuAdd;

            InlineKeyboardMarkup responseInlineKeyboardMarkup =
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuAdd;
            
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuAdd;

            return botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: messageId,
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

        return botClient.EditMessageTextAsync(
            chatId: chatId,
            messageId: messageId,
            text: responseMessageText,
            cancellationToken: cancellationToken
        );
    }
    
     public Task ProcessClickOnInlineButtonInMenuAddChoosing(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        string requestCallBackData = update.CallbackQuery.Data;
        int messageId = update.CallbackQuery.Message.MessageId;

        string responseMessageText = "empty";

        if (requestCallBackData == BotButtonsStorage.ButtonLinkInMenuAdd.CallBackData)
        {
            
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonCategoryInMenuAdd.CallBackData)
        {
            
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonBackwardInMenuAdd.CallBackData)
        {
            responseMessageText = ReplyTextsStorage.MenuMain;
            
            InlineKeyboardMarkup responseInlineKeyboardMarkup = InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain;

            transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;

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