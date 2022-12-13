using LinksKeeperTelegramBot.Model.Connection;
using LinksKeeperTelegramBot.Model.Tables;
using Npgsql;

namespace LinksKeeperTelegramBot.Model;

public class DbManager
{
    public ITableLinks TableLinks { get; private set; }
    public ITableLinksCategories TableLinksCategories { get; private set; }

    public DbManager(ITableLinks tableLinks, ITableLinksCategories tableLinksCategories)
    {
        TableLinks = tableLinks;
        TableLinksCategories = tableLinksCategories;
    }
}