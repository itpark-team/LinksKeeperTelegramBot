namespace LinksKeeperTelegramBot.Router;

public enum State
{
    WaitingCommandStart,
    WaitingClickOnInlineButtonInMenuMain,
    WaitingClickOnInlineButtonInMenuAdd,
    WaitingInputLinkUrlForAdd,
    WaitingInputLinkDescriptionForAdd,
    WaitingClickOnInlineButtonLinkCategoryForAdd,
    WaitingClickOnInlineButtonInMenuApproveAdd,
    WaitingClickOnInlineButtonInMenuAddAnotherLink,
    
    WaitingClickOnInlineButtonLinkCategoryForShow,
    WaitingClickOnInlineButtonLinkCategoryShowLinks,
    
    WaitingInputCategoryForAdd,
    WaitingClickOnInlineButtonInMenuAddAnotherCategory,
    WaitingClickOnInlineButtonInMenuAddCategory,
}