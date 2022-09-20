using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Service.MainMenu;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LinksKeeperTelegramBot.Service;

public class ServicesManager
{
    private Dictionary<State, Func<long, TransmittedData, Update, ITelegramBotClient, CancellationToken, Task>>
        _stateServiceMethodPairs;

    private MainMenuService _mainMenuService;

    public ServicesManager()
    {
        _mainMenuService = new MainMenuService();

        _stateServiceMethodPairs =
            new Dictionary<State, Func<long, TransmittedData, Update, ITelegramBotClient, CancellationToken, Task>>();

        _stateServiceMethodPairs[State.WaitingCommandStart] = _mainMenuService.ProcessCommandStart;
        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuMain] =
            _mainMenuService.ProcessClickOnInlineButtonInMenuMain;
    }

    public Task ProcessBotUpdate(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        try
        {
            var serviceMethod = _stateServiceMethodPairs[transmittedData.State];
            return serviceMethod.Invoke(chatId, transmittedData, update, botClient, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Task(() => { });
        }
    }
}