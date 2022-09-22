using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util.BotButtonsInitializer;
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
        
        responseMessageText = "Доброго времени друзья\nВы в главном меню\nВыберите действие";

        InlineKeyboardMarkup responseInlineKeyboardMarkup = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonAddInMainMenu.Name,
                    BotButtonsStorage.ButtonAddInMainMenu.CallBackData),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonShowInMainMenu.Name,
                    BotButtonsStorage.ButtonShowInMainMenu.CallBackData),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonEditInMainMenu.Name,
                    BotButtonsStorage.ButtonEditInMainMenu.CallBackData),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonDeleteInMainMenu.Name,
                    BotButtonsStorage.ButtonDeleteInMainMenu.CallBackData),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonHowToUseInMainMenu.Name,
                    BotButtonsStorage.ButtonHowToUseInMainMenu.CallBackData),
            },
        });
        
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

        if (requestCallBackData == BotButtonsStorage.ButtonAddInMainMenu.CallBackData)
        {
            responseMessageText = "Нажата клавиша Добавить";
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonShowInMainMenu.CallBackData)
        {
            responseMessageText = "Нажата клавиша Просмотреть";
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonEditInMainMenu.CallBackData)
        {
            responseMessageText = "Нажата клавиша Редактировать";
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonDeleteInMainMenu.CallBackData)
        {
            responseMessageText = "Нажата клавиша Удалить";
        }
        else if (requestCallBackData == BotButtonsStorage.ButtonHowToUseInMainMenu.CallBackData)
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
}