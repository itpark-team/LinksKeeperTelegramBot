
using Npgsql;

namespace LinksKeeperTelegramBot.Model.Tables;

public class TableLinksCategories
{
    private NpgsqlConnection _connection;

    public TableLinksCategories(NpgsqlConnection connection)
    {
        _connection = connection;
    }
}