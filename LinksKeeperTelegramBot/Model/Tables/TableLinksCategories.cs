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

    public LinkCategory getById(int findId)
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
    
    public IEnumerable<LinkCategory> getAll()
    {
        string sqlRequest = "SELECT * FROM links_categories";
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
}