using Npgsql;

namespace LinksKeeperTelegramBot.Model.Tables;

public class TableLinks
{
    private NpgsqlConnection _connection;

    public TableLinks(NpgsqlConnection connection)
    {
        _connection = connection;
    }
}