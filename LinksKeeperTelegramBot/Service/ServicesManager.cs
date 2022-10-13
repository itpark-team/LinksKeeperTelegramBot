using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Service.Links;
using LinksKeeperTelegramBot.Service.MainMenu;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LinksKeeperTelegramBot.Service;

public class ServicesManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private Dictionary<State, Func<long, TransmittedData, Update, ITelegramBotClient, CancellationToken, Task>>
        _stateServiceMethodPairs;

    private MainMenuService _mainMenuService;
    private LinksService _linksService;

    public ServicesManager()
    {
        _mainMenuService = new MainMenuService();
        _linksService = new LinksService();

        _stateServiceMethodPairs =
            new Dictionary<State, Func<long, TransmittedData, Update, ITelegramBotClient, CancellationToken, Task>>();

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
        
    }

    public Task ProcessBotUpdate(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        var serviceMethod = _stateServiceMethodPairs[transmittedData.State];

        Logger.Info(
            $"Вызван метод ProcessBotUpdate Для chatId = {chatId} состояние системы = {transmittedData.State} функция для обработки = {serviceMethod.Method.Name}");

        return serviceMethod.Invoke(chatId, transmittedData, update, botClient, cancellationToken);
    }
}