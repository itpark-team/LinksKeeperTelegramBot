using LinksKeeperTelegramBot.Model;
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

        string responseMessageText = SystemStringsStorage.Empty;

        if (requestMessageText != SystemStringsStorage.CommandStart)
        {
            responseMessageText = DialogsStringsStorage.CommandStartInputErrorInput;

            Logger.Info($"Команда не распознана. Метод ProcessCommandStart. chatId = {chatId}");

            Task taskError = botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                cancellationToken: cancellationToken);

            Logger.Info($"Отправлено сообщение об ошибке пользователю. Метод ProcessCommandStart. chatId = {chatId}");

            return taskError;
        }

        transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;
        
        responseMessageText = DialogsStringsStorage.MenuMain;

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
        string requestCallBackData = update.CallbackQuery.Data;
        

        string responseMessageText = SystemStringsStorage.Empty;

        if (requestCallBackData == BotButtonsStorage.ButtonAddInMenuMain.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuAdd;
            
            responseMessageText = DialogsStringsStorage.MenuAdd;

            InlineKeyboardMarkup responseInlineKeyboardMarkup =
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuAdd;
            
            return botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                replyMarkup: responseInlineKeyboardMarkup,
                cancellationToken: cancellationToken
            );
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonShowInMenuMain.CallBackData)
        {
            if (DbManager.GetInstance().TableLinksCategories.ContaintByChatId(chatId) == false)
            {
                responseMessageText = DialogsStringsStorage.MenuShowNoCategories;
                
                return botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: responseMessageText,
                    cancellationToken: cancellationToken
                );
            }
            
            transmittedData.State = State.WaitingClickOnInlineButtonLinkCategoryForShow;
            
            responseMessageText = DialogsStringsStorage.MenuShow;

            InlineKeyboardMarkup responseInlineKeyboardMarkup =
                InlineKeyboardsMarkupStorage.CreateInlineKeyboardMarkupMenuLinkCategoryForShow(chatId);
            
            return botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                replyMarkup: responseInlineKeyboardMarkup,
                cancellationToken: cancellationToken
            );
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
            text: responseMessageText,
            cancellationToken: cancellationToken
        );
    }

    public Task ProcessClickOnInlineButtonInMenuAddChoosing(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        string requestCallBackData = update.CallbackQuery.Data;

        string responseMessageText = SystemStringsStorage.Empty;

        if (requestCallBackData == BotButtonsStorage.ButtonLinkInMenuAdd.CallBackData)
        {
            transmittedData.State = State.WaitingInputLinkUrlForAdd;
            
            responseMessageText = DialogsStringsStorage.LinkInputUrl;
            
            return botClient.SendTextMessageAsync(
                chatId: chatId,
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
            
            responseMessageText = DialogsStringsStorage.MenuMain;

            InlineKeyboardMarkup responseInlineKeyboardMarkup =
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain;

            return botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                replyMarkup: responseInlineKeyboardMarkup,
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