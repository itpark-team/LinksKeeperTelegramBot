using LinksKeeperTelegramBot.Model.Entities;
using Npgsql;

namespace LinksKeeperTelegramBot.Model.Tables;

public class TableLinks
{
    private NpgsqlConnection _connection;

    public TableLinks(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public void addNew(Link link)
    {
        string sqlRequest = $"INSERT INTO links (url, description, category_id, chat_id) VALUES ('{link.Url}','{link.Description}', {link.CategoryId}, {link.ChatId})";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        command.ExecuteNonQuery();
    }
}