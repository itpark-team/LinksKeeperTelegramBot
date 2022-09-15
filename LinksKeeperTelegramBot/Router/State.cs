namespace LinksKeeperTelegramBot.Router;

public enum State
{
    WaitingCommandStart,
    WaitingClickOnInlineButtonInMenuMain,
    WaitingClickOnInlineButtonInMenuAddChoosing,
    WaitingInputLinkUrlForAdd,
    WaitingInputLinkDescriptionForAdd,
    WaitingClickOnInlineButtonLinkCategoryForAdd,
    WaitingClickOnInlineButtonInMenuApproveAdd,
    WaitingClickOnInlineButtonInMenuAddAnotherLink
}