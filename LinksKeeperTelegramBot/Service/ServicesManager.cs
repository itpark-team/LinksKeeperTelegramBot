using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Service.Handlers;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LinksKeeperTelegramBot.Service;

public class ServicesManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private Dictionary<State, Func<string, TransmittedData, BotTextMessage>>
        _stateServiceMethodPairs;

    private MainMenuService _mainMenuService;
    private LinksService _linksService;

    public ServicesManager()
    {
        _mainMenuService = new MainMenuService();
        _linksService = new LinksService();

        _stateServiceMethodPairs =
            new Dictionary<State, Func<string, TransmittedData, BotTextMessage>>();

        _stateServiceMethodPairs[State.WaitingCommandStart] = _mainMenuService.ProcessCommandStart;

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuMain] =
            _mainMenuService.ProcessClickOnInlineButtonInMenuMain;

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuAdd] =
            _mainMenuService.ProcessClickOnInlineButtonInMenuAddChoosing;

        _stateServiceMethodPairs[State.WaitingInputLinkUrlForAdd] = _linksService.ProcessInputLinkUrlForAdd;

        _stateServiceMethodPairs[State.WaitingInputLinkDescriptionForAdd] =
            _linksService.ProcessInputLinkDescriptionForAdd;

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonLinkCategoryForAdd] =
            _linksService.ProcessClickOnInlineButtonLinkCategoryForAdd;

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuApproveAdd] =
            _linksService.ProcessClickOnInlineButtonInMenuApproveAdd;

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuAddAnotherLink] =
            _linksService.ProcessClickOnInlineButtonInMenuAddAnotherLink;

        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonLinkCategoryForShow] =
            _linksService.ProcessClickOnInlineButtonLinkCategoryForShow;
        
        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonLinkCategoryShowLinks] =
            _linksService.ProcessClickOnInlineButtonLinkCategoryShowLinks;
    }

    public BotTextMessage ProcessBotUpdate(string textData, TransmittedData transmittedData)
    {
        var serviceMethod = _stateServiceMethodPairs[transmittedData.State];

        return serviceMethod.Invoke(textData, transmittedData);
    }
}