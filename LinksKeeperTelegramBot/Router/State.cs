namespace LinksKeeperTelegramBot.Router;

public enum State
{
    CommandStart,
    ClickInMenuMain,
    
    ClickInMenuAdd,
    InputLinkUrlAdd,
    InputLinkDescriptionAdd,
    ClickLinkCategoryAdd,
    ClickInMenuApproveAdd,
    ClickInMenuAnotherLinkAdd,
    InputCategoryAdd,
    ClickInMenuAnotherCategoryAdd,
    ClickInMenuCategoryAdd,
    
    ClickLinkCategoryShow,
    ClickLinkCategoryLinksShow,
    ClickBackwardCategoryLinksShow,

    
    ClickInMenuDelete,
    ClickMenuCategoryDelete,
    ClickLinkCategoryLinksDelete,
    ClickChosenLinkDelete,
    ClickBackwardToChooseCategoryDelete
}