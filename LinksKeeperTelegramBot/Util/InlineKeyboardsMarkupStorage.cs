using LinksKeeperTelegramBot.Model.Entities;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.Util;

public class InlineKeyboardsMarkupStorage
{
    public static InlineKeyboardMarkup MenuMain = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.AddInMenuMain.Name,
                BotButtonsStorage.AddInMenuMain.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ShowInMenuMain.Name,
                BotButtonsStorage.ShowInMenuMain.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.DeleteInMenuMain.Name,
                BotButtonsStorage.DeleteInMenuMain.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithUrl(BotButtonsStorage.HowToUseInMenuMain.Name,
                SystemStringsStorage.InstructionUrl),
        },
    });

    public static InlineKeyboardMarkup MenuAdd = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.LinkInMenuAdd.Name,
                BotButtonsStorage.LinkInMenuAdd.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.CategoryInMenuAdd.Name,
                BotButtonsStorage.CategoryInMenuAdd.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.BackwardInMenuAdd.Name,
                BotButtonsStorage.BackwardInMenuAdd.CallBackData),
        }
    });

    public static InlineKeyboardMarkup CreateMenuLinkCategoryAdd(
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

    public static InlineKeyboardMarkup MenuApproveAdd = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.YesInMenuApproveAdd.Name,
                BotButtonsStorage.YesInMenuApproveAdd.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.NoInMenuApproveAdd.Name,
                BotButtonsStorage.NoInMenuApproveAdd.CallBackData),
        }
    });

    public static InlineKeyboardMarkup AnotherLinkAdd = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.GotoMainMenuInMenuAnotherLinkAdd.Name,
                BotButtonsStorage.GotoMainMenuInMenuAnotherLinkAdd.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.AddOneInMenuAnotherLinkAdd.Name,
                BotButtonsStorage.AddOneInMenuAnotherLinkAdd.CallBackData),
        }
    });
    
    public static InlineKeyboardMarkup BackwardToChooseCategoryInLinksShow = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.BackwardToChooseCategoryInLinksShow.Name,
                BotButtonsStorage.BackwardToChooseCategoryInLinksShow.CallBackData),
        },
    });

    public static InlineKeyboardMarkup CreateMenuLinkCategoryShow(
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
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.BackwardInMenuShow.Name,
                BotButtonsStorage.BackwardInMenuShow.CallBackData)
        });

        return new InlineKeyboardMarkup(keyboardMarkup);
    }

    public static InlineKeyboardMarkup LinksAllShow = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.BackwardInLinksShow.Name,
                BotButtonsStorage.BackwardInLinksShow.CallBackData),
        }
    });

    public static InlineKeyboardMarkup LinksMoreShow = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.MoreInLinksShow.Name,
                BotButtonsStorage.MoreInLinksShow.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.BackwardInLinksShow.Name,
                BotButtonsStorage.BackwardInLinksShow.CallBackData),
        }
    });

    public static InlineKeyboardMarkup MenuAnotherCategoryAdd = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.GotoMainMenuInMenuAnotherCategoryAdd.Name,
                BotButtonsStorage.GotoMainMenuInMenuAnotherCategoryAdd.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.AddOneInMenuAnotherCategoryAdd.Name,
                BotButtonsStorage.AddOneInMenuAnotherCategoryAdd.CallBackData),
        }
    });

    public static InlineKeyboardMarkup MenuCategoryAdd = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.GotoMainMenuInMenuCategoryAdd.Name,
                BotButtonsStorage.GotoMainMenuInMenuCategoryAdd.CallBackData),
        }
    });


    public static InlineKeyboardMarkup MenuDelete = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.LinkInMenuDelete.Name,
                BotButtonsStorage.LinkInMenuDelete.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.CategoryInMenuDelete.Name,
                BotButtonsStorage.CategoryInMenuDelete.CallBackData),
        },
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.BackwardInMenuDelete.Name,
                BotButtonsStorage.BackwardInMenuDelete.CallBackData),
        }
    });

    public static InlineKeyboardMarkup CreateMenuCategoryDelete(
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
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.BackwardInMenuDelete.Name,
                BotButtonsStorage.BackwardInMenuDelete.CallBackData)
        });

        return new InlineKeyboardMarkup(keyboardMarkup);
    }


    public static InlineKeyboardMarkup CreateMenuLinkCategoryLinkDelete(
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

    public static InlineKeyboardMarkup CreateLinksAllDelete(IEnumerable<Link> links)
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
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.BackwardInLinkDelete.Name,
                BotButtonsStorage.BackwardInLinkDelete.CallBackData)
        });

        return new InlineKeyboardMarkup(keyboardMarkup);
    }
    
    public static InlineKeyboardMarkup CreateLinksAllMoreDelete(IEnumerable<Link> links)
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
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.MoreInLinkDelete.Name,
                BotButtonsStorage.MoreInLinkDelete.CallBackData)
        });
        
        keyboardMarkup.Add(new()
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.BackwardInLinkDelete.Name,
                BotButtonsStorage.BackwardInLinkDelete.CallBackData)
        });

        return new InlineKeyboardMarkup(keyboardMarkup);
    }
    
    
    public static InlineKeyboardMarkup BackwardToChooseCategoryInDeleteLink = new(new[]
    {
        new[]
        {
            InlineKeyboardButton.WithCallbackData(BotButtonsStorage.ButtonNoLinksInChooseCategoryInDeleteLink.Name,
                BotButtonsStorage.ButtonNoLinksInChooseCategoryInDeleteLink.CallBackData),
        },
    });
}