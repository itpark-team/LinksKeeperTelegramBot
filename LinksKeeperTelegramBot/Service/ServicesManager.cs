using LinksKeeperTelegramBot.BotInitializer;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Connection;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Service.MenuPoints;
using LinksKeeperTelegramBot.Util;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LinksKeeperTelegramBot.Service;

public class ServicesManager
{
    private Dictionary<State, Func<string, TransmittedData, BotTextMessage>>
        _methods;

    private MainMenuService _mainMenuService;
    private AddService _addService;
    private ShowService _showService;
    private DeleteService _deleteService;
    
    public ServicesManager()
    {
        DbConnector dbConnector = new DbConnector(
            SystemStringsStorage.DbHost,
            SystemStringsStorage.DbUsername,
            SystemStringsStorage.DbPassword,
            SystemStringsStorage.DbDatabasename);

        ITableLinks tableLinks = new TableLinks(dbConnector.Connection);
        ITableLinksCategories tableLinksCategories = new TableLinksCategories(dbConnector.Connection);

        DbManager dbManager = new DbManager(tableLinks, tableLinksCategories);

        _mainMenuService = new MainMenuService(dbManager);
        _addService = new AddService(dbManager);
        _showService = new ShowService(dbManager);
        _deleteService = new DeleteService(dbManager);

        _methods =
            new Dictionary<State, Func<string, TransmittedData, BotTextMessage>>();

        #region MenuMainService

        _methods[State.CommandStart] = _mainMenuService.ProcessCommandStart;

        _methods[State.ClickInMenuMain] =
            _mainMenuService.ProcessClickInMenuMain;

        #endregion

        #region MenuPointAddServices

        _methods[State.ClickInMenuAdd] =
            _addService.ProcessClickInMenuAdd;

        _methods[State.InputLinkUrlAdd] = _addService.ProcessInputLinkUrlAdd;

        _methods[State.InputLinkDescriptionAdd] =
            _addService.ProcessInputLinkDescriptionAdd;

        _methods[State.ClickLinkCategoryAdd] =
            _addService.ProcessClickLinkCategoryAdd;

        _methods[State.ClickInMenuApproveAdd] =
            _addService.ProcessClickInMenuApproveAdd;

        _methods[State.ClickInMenuAnotherLinkAdd] =
            _addService.ProcessClickInMenuAnotherLinkAdd;

        _methods[State.InputCategoryAdd] =
            _addService.ProcessInputCategoryAdd;

        _methods[State.ClickInMenuAnotherCategoryAdd] =
            _addService.ProcessClickInMenuAnotherCategoryAdd;

        _methods[State.ClickInMenuCategoryAdd] =
            _addService.ProcessClickInMenuCategoryAdd;

        #endregion

        #region MenuPointShowServices

        _methods[State.ClickLinkCategoryShow] =
            _showService.ProcessClickLinkCategoryShow;

        _methods[State.ClickLinkCategoryLinksShow] =
            _showService.ProcessClickLinkCategoryLinksShow;

        #endregion

        #region MenuPointDeleteServices

        _methods[State.ClickInMenuDelete] =
            _deleteService.ProcessClickInMenuDelete;


        _methods[State.ClickMenuCategoryDelete] =
            _deleteService.ProcessClickMenuCategoryDelete;

        _methods[State.ClickLinkCategoryLinksDelete] =
            _deleteService.ProcessClickLinkCategoryLinksDelete;

        _methods[State.ClickChosenLinkDelete] =
            _deleteService.ProcessClickChosenLinkDelete;

        #endregion
    }

    public BotTextMessage ProcessBotUpdate(string textData, TransmittedData transmittedData)
    {
        var serviceMethod = _methods[transmittedData.State];

        return serviceMethod.Invoke(textData, transmittedData);
    }
}