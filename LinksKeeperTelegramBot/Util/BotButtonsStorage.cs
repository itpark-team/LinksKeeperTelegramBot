namespace LinksKeeperTelegramBot.Util;

public class BotButtonsStorage
{
    #region ButtonsInMenuMain
    public static BotButton AddInMenuMain { get; } = new BotButton("Добавить", "AddInMenuMain");
    public static BotButton ShowInMenuMain { get; } = new BotButton("Просмотреть", "ShowInMenuMain");
    public static BotButton DeleteInMenuMain { get; } = new BotButton("Удалить", "DeleteInMenuMain");
    public static BotButton HowToUseInMenuMain { get; } = new BotButton("Как пользоваться", "HowToUseInMenuMain");
    #endregion

    #region ButtonsInMenuAdd
    public static BotButton LinkInMenuAdd { get; } = new BotButton("Ссылку", "LinkInMenuAdd");
    public static BotButton CategoryInMenuAdd { get; } = new BotButton("Категория", "CategoryInMenuAdd");
    public static BotButton BackwardInMenuAdd { get; } = new BotButton("Назад", "BackwardInMenuAdd");
    #endregion
    
    #region ButtonsInMenuApproveAdd
    public static BotButton YesInMenuApproveAdd { get; } = new BotButton("Да", "YesInMenuApproveAdd");
    public static BotButton NoInMenuApproveAdd { get; } = new BotButton("Нет", "NoInMenuApproveAdd");
    #endregion
    
    #region ButtonsInMenuAddAnotherLink 
    public static BotButton GotoMainMenuInMenuAnotherLinkAdd { get; } = new BotButton("В главное меню", "GotoMainMenuInMenuAnotherLinkAdd");
    public static BotButton AddOneInMenuAnotherLinkAdd { get; } = new BotButton("Добавить ещё", "AddOneInMenuAnotherLinkAdd");
    #endregion
    
    #region ButtonsInMenuMenuShow 
    public static BotButton BackwardInMenuShow { get; } = new BotButton("Назад", "BackwardInMenuShow");
    
    #endregion
    
    #region ButtonsInShowLinks 
    public static BotButton BackwardInLinksShow { get; } = new BotButton("Назад", "BackwardInLinksShow");
    public static BotButton MoreInLinksShow { get; } = new BotButton("Ещё", "MoreInLinksShow");
    
    public static BotButton BackwardToChooseCategoryInLinksShow { get; } = new BotButton("Назад", "BackwardToChooseCategoryInLinksShow");
    #endregion
    
    #region ButtonsInMenuAddAnotherCategory
    public static BotButton GotoMainMenuInMenuAnotherCategoryAdd { get; } = new BotButton("В главное меню", "GotoMainMenuInMenuAnotherCategoryAdd");
    public static BotButton AddOneInMenuAnotherCategoryAdd { get; } = new BotButton("Добавить ещё", "AddOneInMenuAnotherCategoryAdd");
    #endregion

    #region ButtonsInMenuCategory
    public static BotButton GotoMainMenuInMenuCategoryAdd { get; } = new BotButton("В главное меню", "GotoMainMenuInMenuCategoryAdd");
    #endregion
    
    #region ButtonsInMenuDelete
    public static BotButton LinkInMenuDelete { get; } = new BotButton("Ссылку", "LinkInMenuDelete");
    public static BotButton CategoryInMenuDelete { get; } = new BotButton("Категория", "CategoryInMenuDelete");
    public static BotButton BackwardInMenuDelete { get; } = new BotButton("Назад", "BackwardInMenuDelete");
    #endregion
    
    #region ButtonsInDeleteCategory
    public static BotButton BackwardCategoryDelete { get; } = new BotButton("Назад", "BackwardCategoryDelete");
    #endregion
    
    #region ButtonsInChooseCategoryInDeleteLink
    public static BotButton ButtonInChooseCategoryInDeleteLink { get; } = new BotButton("Назад", "ButtonsInChooseCategoryInDeleteLink");
    
    public static BotButton ButtonNoLinksInChooseCategoryInDeleteLink { get; } = new BotButton("Назад", "ButtonNoLinksInChooseCategoryInDeleteLink");
    
    #endregion
    
    #region ButtonsInDeleteLink
    
    public static BotButton MoreInLinkDelete { get; } = new BotButton("Ещё", "MoreInLinkDelete");
    
    public static BotButton BackwardInLinkDelete { get; } = new BotButton("Назад", "BackwardInLinkDelete");
    
    #endregion
    
}