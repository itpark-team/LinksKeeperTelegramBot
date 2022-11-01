namespace LinksKeeperTelegramBot.Util;

public class BotButtonsStorage
{
    #region ButtonsInMenuMain
    public static BotButton ButtonAddInMenuMain { get; } = new BotButton("Добавить", "ButtonAddInMenuMain");
    public static BotButton ButtonShowInMenuMain { get; } = new BotButton("Просмотреть", "ButtonShowInMenuMain");
    public static BotButton ButtonDeleteInMenuMain { get; } = new BotButton("Удалить", "ButtonDeleteInMenuMain");
    public static BotButton ButtonHowToUseInMenuMain { get; } = new BotButton("Как пользоваться", "ButtonHowToUseInMenuMain");
    #endregion

    #region ButtonsInMenuAdd
    public static BotButton ButtonLinkInMenuAdd { get; } = new BotButton("Ссылку", "ButtonLinkInMenuAdd");
    public static BotButton ButtonCategoryInMenuAdd { get; } = new BotButton("Категория", "ButtonCategoryInMenuAdd");
    public static BotButton ButtonBackwardInMenuAdd { get; } = new BotButton("Назад", "ButtonBackwardInMenuAdd");
    #endregion
    
    #region ButtonsInMenuApproveAdd
    public static BotButton ButtonYesInMenuApproveAdd { get; } = new BotButton("Да", "ButtonYesInMenuApproveAdd");
    public static BotButton ButtonNoInMenuApproveAdd { get; } = new BotButton("Нет", "ButtonNoInMenuApproveAdd");
    #endregion
    
    #region ButtonsInMenuAddAnotherLink 
    public static BotButton ButtonGotoMainMenuInMenuAddAnotherLink { get; } = new BotButton("В главное меню", "ButtonMenuInMenuAddAnotherLink");
    public static BotButton ButtonAddOneInMenuAddAnotherLink { get; } = new BotButton("Добавить ещё", "ButtonAddOneInMenuAddAnotherLink");
    #endregion
    
    #region ButtonsInMenuMenuShow 
    public static BotButton ButtonBackwardInMenuShow { get; } = new BotButton("Назад", "ButtonBackwardInMenuShow");
    
    #endregion
    
    #region ButtonsInShowLinks 
    public static BotButton ButtonBackwardInShowLinks { get; } = new BotButton("Назад", "ButtonBackwardInShowLinks");
    public static BotButton ButtonMoreInShowLinks { get; } = new BotButton("Ещё", "ButtonMoreInShowLinks");
    #endregion
    
    #region ButtonsInMenuAddAnotherCategory
    public static BotButton ButtonGotoMainMenuInMenuAddAnotherCategory { get; } = new BotButton("В главное меню", "ButtonMenuInMenuAddAnotherCategory");
    public static BotButton ButtonAddOneInMenuAddAnotherCategory { get; } = new BotButton("Добавить ещё", "ButtonAddOneInMenuAddAnotherCategory");
    #endregion

    #region ButtonsInMenuCategory
    public static BotButton ButtonGotoMainMenuInMenuAddCategory { get; } = new BotButton("В главное меню", "ButtonGotoMainMenuInMenuAddCategory");
    #endregion
    
    #region ButtonsInMenuDelete
    public static BotButton ButtonLinkInMenuDelete { get; } = new BotButton("Ссылку", "ButtonLinkInMenuDelete");
    public static BotButton ButtonCategoryInMenuDelete { get; } = new BotButton("Категория", "ButtonCategoryInMenuDelete");
    public static BotButton ButtonBackwardInMenuDelete { get; } = new BotButton("Назад", "ButtonBackwardInMenuDelete");
    #endregion
    
    #region ButtonsInDeleteCategory
    public static BotButton ButtonBackwardDeleteCategory { get; } = new BotButton("Назад", "ButtonBackwardDeleteCategory");
    #endregion
    
    #region ButtonsInChooseCategoryInDeleteLink
    public static BotButton ButtonInChooseCategoryInDeleteLink { get; } = new BotButton("Назад", "ButtonsInChooseCategoryInDeleteLink");
    
    #endregion
    
    #region ButtonsInDeleteLink
    
    public static BotButton ButtonMoreInDeleteLink { get; } = new BotButton("Ещё", "ButtonMoreInDeleteLink");
    
    public static BotButton ButtonBackwardInDeleteLink { get; } = new BotButton("Назад", "ButtonBackwardInDeleteLink");
    
    #endregion
    
}