using System.Dynamic;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Entities;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Util;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Util;

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
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonDeleteInMenuMain.Name,
                BotButtonsStorage.ButtonDeleteInMenuMain.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithUrl(BotButtonsStorage.ButtonHowToUseInMenuMain.Name,
                SystemStringsStorage.InstructionUrl),
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

    public static InlineKeyboardMarkup CreateInlineKeyboardMarkupMenuLinkCategoryForAdd(
        IEnumerable<LinkCategory> linkCategories)
    {
        List<List<InlineKeyboardButton>> keyboardMarkup = new List<List<InlineKeyboardButton>>();

        foreach (LinkCategory linkCategory in linkCategories)
        {
            keyboardMarkup.Add(
                new()
                {
                    InlineKeyboardButton.WithCallbackData(linkCategory.Name,
                        SystemStringsStorage.LinkCategoryIdText + linkCategory.Id)
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

    public static InlineKeyboardMarkup CreateInlineKeyboardMarkupMenuLinkCategoryForShow(
        IEnumerable<LinkCategory> linkCategories)
    {
        List<List<InlineKeyboardButton>> keyboardMarkup = new List<List<InlineKeyboardButton>>();

        foreach (LinkCategory linkCategory in linkCategories)
        {
            keyboardMarkup.Add(
                new()
                {
                    InlineKeyboardButton.WithCallbackData(linkCategory.Name,
                        SystemStringsStorage.LinkCategoryIdText + linkCategory.Id)
                }
            );
        }

        keyboardMarkup.Add(new()
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonBackwardInMenuShow.Name,
                BotButtonsStorage.ButtonBackwardInMenuShow.CallBackData)
        });

        return new InlineKeyboardMarkup(keyboardMarkup);
    }

    public static InlineKeyboardMarkup InlineKeyboardMarkupShowLinksAll = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonBackwardInShowLinks.Name,
                BotButtonsStorage.ButtonBackwardInShowLinks.CallBackData),
        }
    });

    public static InlineKeyboardMarkup InlineKeyboardMarkupShowLinksMore = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonMoreInShowLinks.Name,
                BotButtonsStorage.ButtonMoreInShowLinks.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonBackwardInShowLinks.Name,
                BotButtonsStorage.ButtonBackwardInShowLinks.CallBackData),
        }
    });

    public static InlineKeyboardMarkup InlineKeyboardMarkupMenuAddAnotherCategory = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonGotoMainMenuInMenuAddAnotherCategory.Name,
                BotButtonsStorage.ButtonGotoMainMenuInMenuAddAnotherCategory.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonAddOneInMenuAddAnotherCategory.Name,
                BotButtonsStorage.ButtonAddOneInMenuAddAnotherCategory.CallBackData),
        }
    });

    public static InlineKeyboardMarkup InlineKeyboardMarkupMenuAddCategory = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonGotoMainMenuInMenuAddCategory.Name,
                BotButtonsStorage.ButtonGotoMainMenuInMenuAddCategory.CallBackData),
        }
    });


    public static InlineKeyboardMarkup InlineKeyboardMarkupMenuDelete = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonLinkInMenuDelete.Name,
                BotButtonsStorage.ButtonLinkInMenuDelete.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonCategoryInMenuDelete.Name,
                BotButtonsStorage.ButtonCategoryInMenuDelete.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonBackwardInMenuDelete.Name,
                BotButtonsStorage.ButtonBackwardInMenuDelete.CallBackData),
        }
    });

    public static InlineKeyboardMarkup CreateInlineKeyboardMarkupMenuDeleteCategory(
        IEnumerable<LinkCategory> linkCategories)
    {
        List<List<InlineKeyboardButton>> keyboardMarkup = new List<List<InlineKeyboardButton>>();

        foreach (LinkCategory linkCategory in linkCategories)
        {
            keyboardMarkup.Add(
                new()
                {
                    InlineKeyboardButton.WithCallbackData(linkCategory.Name,
                        SystemStringsStorage.LinkCategoryIdText + linkCategory.Id)
                }
            );
        }

        keyboardMarkup.Add(new()
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonBackwardInMenuDelete.Name,
                BotButtonsStorage.ButtonBackwardInMenuDelete.CallBackData)
        });

        return new InlineKeyboardMarkup(keyboardMarkup);
    }


    public static InlineKeyboardMarkup CreateInlineKeyboardMarkupMenuLinkCategoryForDeleteLink(
        IEnumerable<LinkCategory> linkCategories)
    {
        List<List<InlineKeyboardButton>> keyboardMarkup = new List<List<InlineKeyboardButton>>();

        foreach (LinkCategory linkCategory in linkCategories)
        {
            keyboardMarkup.Add(
                new()
                {
                    InlineKeyboardButton.WithCallbackData(linkCategory.Name,
                        SystemStringsStorage.LinkCategoryIdText + linkCategory.Id)
                }
            );
        }

        keyboardMarkup.Add(new()
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonInChooseCategoryInDeleteLink.Name,
                BotButtonsStorage.ButtonInChooseCategoryInDeleteLink.CallBackData)
        });

        return new InlineKeyboardMarkup(keyboardMarkup);
    }

    public static InlineKeyboardMarkup CreateInlineKeyboardMarkupDeleteLinksAll(IEnumerable<Link> links)
    {
        List<List<InlineKeyboardButton>> keyboardMarkup = new List<List<InlineKeyboardButton>>();

        foreach (Link link in links)
        {
            keyboardMarkup.Add(
                new()
                {
                    InlineKeyboardButton.WithCallbackData(SystemStringsStorage.DeleteLinkPhrase + link.Id,
                        SystemStringsStorage.LinkIdText + link.Id)
                }
            );
        }

        keyboardMarkup.Add(new()
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonBackwardInDeleteLink.Name,
                BotButtonsStorage.ButtonBackwardInDeleteLink.CallBackData)
        });

        return new InlineKeyboardMarkup(keyboardMarkup);
    }
    
    public static InlineKeyboardMarkup CreateInlineKeyboardMarkupDeleteLinksAllMore(IEnumerable<Link> links)
    {
        List<List<InlineKeyboardButton>> keyboardMarkup = new List<List<InlineKeyboardButton>>();

        foreach (Link link in links)
        {
            keyboardMarkup.Add(
                new()
                {
                    InlineKeyboardButton.WithCallbackData(SystemStringsStorage.DeleteLinkPhrase + link.Id,
                        SystemStringsStorage.LinkIdText + link.Id)
                }
            );
        }

        keyboardMarkup.Add(new()
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonMoreInDeleteLink.Name,
                BotButtonsStorage.ButtonMoreInDeleteLink.CallBackData)
        });
        
        keyboardMarkup.Add(new()
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonBackwardInDeleteLink.Name,
                BotButtonsStorage.ButtonBackwardInDeleteLink.CallBackData)
        });

        return new InlineKeyboardMarkup(keyboardMarkup);
    }
}