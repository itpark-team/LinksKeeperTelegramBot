using LinksKeeperTelegramBot.Model.Entities;
using Npgsql;

namespace LinksKeeperTelegramBot.Model.Tables;

public class TableLinksCategories
{
    private NpgsqlConnection _connection;

    public TableLinksCategories(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public LinkCategory GetById(int findId)
    {
        string sqlRequest = $"SELECT * FROM links_categories WHERE id={findId}";
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        dataReader.Read();

        int id = dataReader.GetInt32(dataReader.GetOrdinal("id"));
        string name = dataReader.GetString(dataReader.GetOrdinal("name"));

        dataReader.Close();

        return new LinkCategory()
        {
            Id = id,
            Name = name
        };
    }

    public bool ContaintByChatId(long chatId)
    {
        string sqlRequest = $"SELECT * FROM links_categories WHERE chat_id={chatId}";
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        bool exist = dataReader.HasRows;

        dataReader.Close();

        return exist;
    }

    public void AddNew(LinkCategory linkCategory)
    {
        string sqlRequest =
            $"INSERT INTO links_categories (name, chat_id) VALUES ('{linkCategory.Name}', {linkCategory.ChatId})";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        command.ExecuteNonQuery();
    }

    public IEnumerable<LinkCategory> GetAllByChatId(long chatId)
    {
        string sqlRequest = $"SELECT * FROM links_categories WHERE chat_id={chatId} ORDER BY id ASC";
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        List<LinkCategory> linkCategories = new List<LinkCategory>();

        while (dataReader.Read())
        {
            int id = dataReader.GetInt32(dataReader.GetOrdinal("id"));
            string name = dataReader.GetString(dataReader.GetOrdinal("name"));

            linkCategories.Add(new LinkCategory()
            {
                Id = id,
                Name = name
            });
        }

        dataReader.Close();

        return linkCategories;
    }

    public void DeleteById(int id)
    {
        string sqlRequest = $"DELETE FROM links_categories WHERE id={id}";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        command.ExecuteNonQuery();
    }
}