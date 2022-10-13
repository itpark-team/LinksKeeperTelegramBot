namespace LinksKeeperTelegramBot.Util.BotButtonsInitializer;

public class BotButtonsStorage
{
    #region ButtonsInMenuMain
    public static BotButton ButtonAddInMenuMain { get; } = new BotButton("Добавить", "ButtonAddInMenuMain");
    public static BotButton ButtonShowInMenuMain { get; } = new BotButton("Просмотреть", "ButtonShowInMenuMain");
    public static BotButton ButtonEditInMenuMain { get; } = new BotButton("Редактировать", "ButtonEditInMenuMain");
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
}