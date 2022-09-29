using LinksKeeperTelegramBot.Util.BotButtonsInitializer;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Util.InlineKeyboardsMarkupInitializer;

public class InlineKeyboardsMarkupStorage
{
    public static InlineKeyboardMarkup InlineKeyboardMarkupMenuMain = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonAddInMenuMain.Name,
                BotButtonsStorage.ButtonAddInMenuMain.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonShowInMenuMain.Name,
                BotButtonsStorage.ButtonShowInMenuMain.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonEditInMenuMain.Name,
                BotButtonsStorage.ButtonEditInMenuMain.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonDeleteInMenuMain.Name,
                BotButtonsStorage.ButtonDeleteInMenuMain.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonHowToUseInMenuMain.Name,
                BotButtonsStorage.ButtonHowToUseInMenuMain.CallBackData),
        },
    });
    
    
    public static InlineKeyboardMarkup InlineKeyboardMarkupMenuAdd = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonLinkInMenuAdd.Name,
                BotButtonsStorage.ButtonLinkInMenuAdd.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonCategoryInMenuAdd.Name,
                BotButtonsStorage.ButtonCategoryInMenuAdd.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonBackwardInMenuAdd.Name,
                BotButtonsStorage.ButtonBackwardInMenuAdd.CallBackData),
        }
    });
}