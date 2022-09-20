using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util.BotButtonsInitializer;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Service.MainMenu;

public class MainMenuService
{
    public Task ProcessCommandStart(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        string requestMessageText = update.Message.Text;

        string responseMessageText = "empty";

        if (requestMessageText != "/start")
        {
            responseMessageText = "Команда не распознана. Для начала работы с ботом введите /start";

            return botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                cancellationToken: cancellationToken);
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
        
        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            replyMarkup: responseInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
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