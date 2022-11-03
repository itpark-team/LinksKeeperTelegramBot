using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Service.Handlers;
using LinksKeeperTelegramBot.Service.Services;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LinksKeeperTelegramBot.Service;

public class ServicesManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private Dictionary<State, Func<string, TransmittedData, BotTextMessage>>
        _stateServiceMethodPairs;

    private MenuMainService _menuMainService;
    private MenuPointAddServices _menuPointAddServices;
    private MenuPointShowServices _menuPointShowServices;
    private MenuPointDeleteServices _menuPointDeleteServices;

    public ServicesManager()
    {
        _menuMainService = new MenuMainService();
        _menuPointAddServices = new MenuPointAddServices();
        _menuPointShowServices = new MenuPointShowServices();
        _menuPointDeleteServices = new MenuPointDeleteServices();

        _stateServiceMethodPairs =
            new Dictionary<State, Func<string, TransmittedData, BotTextMessage>>();

        #region MenuMainService

        _stateServiceMethodPairs[State.WaitingCommandStart] = _menuMainService.ProcessCommandStart;

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuMain] =
            _menuMainService.ProcessClickOnInlineButtonInMenuMain;

        #endregion
        
        #region MenuPointAddServices

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuAdd] =
            _menuPointAddServices.ProcessClickOnInlineButtonInMenuAdd;
        
        _stateServiceMethodPairs[State.WaitingInputLinkUrlForAdd] = _menuPointAddServices.ProcessInputLinkUrlForAdd;

        _stateServiceMethodPairs[State.WaitingInputLinkDescriptionForAdd] =
            _menuPointAddServices.ProcessInputLinkDescriptionForAdd;

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonLinkCategoryForAdd] =
            _menuPointAddServices.ProcessClickOnInlineButtonLinkCategoryForAdd;

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuApproveAdd] =
            _menuPointAddServices.ProcessClickOnInlineButtonInMenuApproveAdd;

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuAddAnotherLink] =
            _menuPointAddServices.ProcessClickOnInlineButtonInMenuAddAnotherLink;

        _stateServiceMethodPairs[State.WaitingInputCategoryForAdd] =
            _menuPointAddServices.ProcessInputCategoryForAdd;

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuAddAnotherCategory] =
            _menuPointAddServices.ProcessClickOnInlineButtonInMenuAddAnotherCategory;
        
        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuAddCategory] =
            _menuPointAddServices.ProcessClickOnInlineButtonInMenuAddCategory;


        #endregion

        #region MenuPointShowServices
        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonLinkCategoryForShow] =
            _menuPointShowServices.ProcessClickOnInlineButtonLinkCategoryForShow;

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonLinkCategoryShowLinks] =
            _menuPointShowServices.ProcessClickOnInlineButtonLinkCategoryShowLinks;
        #endregion
        
        #region MenuPointDeleteServices
        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuDelete] =
            _menuPointDeleteServices.ProcessClickOnInlineButtonInMenuDelete;
        

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonMenuDeleteCategory] =
            _menuPointDeleteServices.ProcessClickOnInlineButtonInMenuDeleteCategory;
        
        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonLinkCategoryForDeleteLinks] =
            _menuPointDeleteServices.ProcessClickOnInlineButtonLinkCategoryForDeleteLinks;
        
        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonDeleteChosenLink] =
            _menuPointDeleteServices.ProcessClickOnInlineButtonDeleteChosenLink;
        #endregion
    }

    public BotTextMessage ProcessBotUpdate(string textData, TransmittedData transmittedData)
    {
        var serviceMethod = _stateServiceMethodPairs[transmittedData.State];

        return serviceMethod.Invoke(textData, transmittedData);
    }
}