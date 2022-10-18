using System.Dynamic;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Util.BotButtonsInitializer;
using LinksKeeperTelegramBot.Util.Settings;
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

    public static InlineKeyboardMarkup CreateInlineKeyboardMarkupMenuLinkCategoryForAdd(long chatId)
    {
        TableLinksCategories tableLinksCategories = DbManager.GetInstance().TableLinksCategories;

        if (tableLinksCategories.ContaintByChatId(chatId)==false)
        {
            tableLinksCategories.AddNew(new LinkCategory()
            {
                Name = SystemStringsStorage.FirstLinkCategory,
                ChatId = chatId
            });
        }
        
        IEnumerable<LinkCategory> linkCategories = tableLinksCategories.GetAllChatId(chatId);

        List<List<InlineKeyboardButton>> keyboardMarkup = new List<List<InlineKeyboardButton>>();

        foreach (LinkCategory linkCategory in linkCategories)
        {
            keyboardMarkup.Add(
                new()
                {
                    InlineKeyboardButton.WithCallbackData(linkCategory.Name, linkCategory.Id.ToString())
                }
            );
        }

        return new InlineKeyboardMarkup(keyboardMarkup);
    }
    
    public static InlineKeyboardMarkup InlineKeyboardMarkupMenuApproveAdd = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonYesInMenuApproveAdd.Name,
                BotButtonsStorage.ButtonYesInMenuApproveAdd.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonNoInMenuApproveAdd.Name,
                BotButtonsStorage.ButtonNoInMenuApproveAdd.CallBackData),
        }
    });
    
    public static InlineKeyboardMarkup InlineKeyboardMarkupMenuAddAnotherLink = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonGotoMainMenuInMenuAddAnotherLink.Name,
                BotButtonsStorage.ButtonGotoMainMenuInMenuAddAnotherLink.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonAddOneInMenuAddAnotherLink.Name,
                BotButtonsStorage.ButtonAddOneInMenuAddAnotherLink.CallBackData),
        }
    });
}