using Npgsql;

namespace LinksKeeperTelegramBot.Model.Connection;

public class DbConnector
{
    public NpgsqlConnection Connection { private set; get; }

    public DbConnector(string host, string username, string password, string databasename)
    {
        string _connectionString =
            $"Host={host};Username={username};Password={password};Database={databasename}";

        Connection = new NpgsqlConnection(_connectionString);
        Connection.Open();
    }
}