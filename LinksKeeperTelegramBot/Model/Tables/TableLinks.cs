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

    public void AddNew(Link link)
    {
        string sqlRequest =
            $"INSERT INTO links (url, description, category_id, chat_id) VALUES ('{link.Url}','{link.Description}', {link.CategoryId}, {link.ChatId})";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        command.ExecuteNonQuery();
    }

    public IEnumerable<Link> GetAllByCategoryId(int findCategoryId)
    {
        string sqlRequest = $"SELECT * FROM links WHERE category_id={findCategoryId} ORDER BY id ASC";
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        List<Link> links = new List<Link>();

        while (dataReader.Read())
        {
            int id = dataReader.GetInt32(dataReader.GetOrdinal("id"));
            string url = dataReader.GetString(dataReader.GetOrdinal("url"));
            string description = dataReader.GetString(dataReader.GetOrdinal("description"));
            int categoryId = dataReader.GetInt32(dataReader.GetOrdinal("category_id"));
            long chatId = dataReader.GetInt64(dataReader.GetOrdinal("chat_id"));

            links.Add(new Link()
            {
                Id = id,
                Url = url,
                Description = description,
                CategoryId = categoryId,
                ChatId = chatId
            });
        }

        dataReader.Close();

        return links;
    }

    public IEnumerable<Link> GetAllByCategoryIdWithStartLinkId(int findCategoryId, int startLinkId)
    {
        string sqlRequest =
            $"SELECT * FROM links WHERE category_id={findCategoryId} AND id>={startLinkId} ORDER BY id ASC";
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        List<Link> links = new List<Link>();

        while (dataReader.Read())
        {
            int id = dataReader.GetInt32(dataReader.GetOrdinal("id"));
            string url = dataReader.GetString(dataReader.GetOrdinal("url"));
            string description = dataReader.GetString(dataReader.GetOrdinal("description"));
            int categoryId = dataReader.GetInt32(dataReader.GetOrdinal("category_id"));
            long chatId = dataReader.GetInt64(dataReader.GetOrdinal("chat_id"));

            links.Add(new Link()
            {
                Id = id,
                Url = url,
                Description = description,
                CategoryId = categoryId,
                ChatId = chatId
            });
        }

        dataReader.Close();

        return links;
    }
    
    public void DeleteById(int id)
    {
        string sqlRequest = $"DELETE FROM links WHERE id={id}";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        command.ExecuteNonQuery();
    }
}