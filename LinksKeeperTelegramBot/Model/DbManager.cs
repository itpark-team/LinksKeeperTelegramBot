using LinksKeeperTelegramBot.Model.Connection;
using LinksKeeperTelegramBot.Model.Tables;
using Npgsql;

namespace LinksKeeperTelegramBot.Model;

public class DbManager
{
    public TableLinks TableLinks { get; private set; }
    public TableLinksCategories TableLinksCategories { get; private set; }

    public DbManager()
    {
        NpgsqlConnection connection = DbConnector.GetInstance().Connection;

        TableLinks = new TableLinks(connection);
        TableLinksCategories = new TableLinksCategories(connection);
    }

    private static DbManager _dbManager = null;

    public static DbManager GetInstance()
    {
        if (_dbManager == null)
        {
            _dbManager = new DbManager();
        }

        return _dbManager;
    }
}