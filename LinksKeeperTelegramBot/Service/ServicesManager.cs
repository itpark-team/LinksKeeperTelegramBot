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
    private AddServices _addServices;
    private ShowServices _showServices;
    private DeleteServices _deleteServices;
    
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
        _addServices = new AddServices(dbManager);
        _showServices = new ShowServices(dbManager);
        _deleteServices = new DeleteServices(dbManager);

        _methods =
            new Dictionary<State, Func<string, TransmittedData, BotTextMessage>>();

        #region MenuMainService

        _methods[State.CommandStart] = _mainMenuService.ProcessCommandStart;

        _methods[State.ClickInMenuMain] =
            _mainMenuService.ProcessClickInMenuMain;

        #endregion

        #region MenuPointAddServices

        _methods[State.ClickInMenuAdd] =
            _addServices.ProcessClickInMenuAdd;

        _methods[State.InputLinkUrlAdd] = _addServices.ProcessInputLinkUrlAdd;

        _methods[State.InputLinkDescriptionAdd] =
            _addServices.ProcessInputLinkDescriptionAdd;

        _methods[State.ClickLinkCategoryAdd] =
            _addServices.ProcessClickLinkCategoryAdd;

        _methods[State.ClickInMenuApproveAdd] =
            _addServices.ProcessClickInMenuApproveAdd;

        _methods[State.ClickInMenuAnotherLinkAdd] =
            _addServices.ProcessClickInMenuAnotherLinkAdd;

        _methods[State.InputCategoryAdd] =
            _addServices.ProcessInputCategoryAdd;

        _methods[State.ClickInMenuAnotherCategoryAdd] =
            _addServices.ProcessClickInMenuAnotherCategoryAdd;

        _methods[State.ClickInMenuCategoryAdd] =
            _addServices.ProcessClickInMenuCategoryAdd;

        #endregion

        #region MenuPointShowServices

        _methods[State.ClickLinkCategoryShow] =
            _showServices.ProcessClickLinkCategoryShow;

        _methods[State.ClickLinkCategoryLinksShow] =
            _showServices.ProcessClickLinkCategoryLinksShow;

        #endregion

        #region MenuPointDeleteServices

        _methods[State.ClickInMenuDelete] =
            _deleteServices.ProcessClickInMenuDelete;


        _methods[State.ClickMenuCategoryDelete] =
            _deleteServices.ProcessClickMenuCategoryDelete;

        _methods[State.ClickLinkCategoryLinksDelete] =
            _deleteServices.ProcessClickLinkCategoryLinksDelete;

        _methods[State.ClickChosenLinkDelete] =
            _deleteServices.ProcessClickChosenLinkDelete;

        #endregion
    }

    public BotTextMessage ProcessBotUpdate(string textData, TransmittedData transmittedData)
    {
        var serviceMethod = _methods[transmittedData.State];

        return serviceMethod.Invoke(textData, transmittedData);
    }
}