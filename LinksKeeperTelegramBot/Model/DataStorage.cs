namespace LinksKeeperTelegramBot.Model;

public class DataStorage
{
    private Dictionary<string, object> data;

    public DataStorage()
    {
        data = new Dictionary<string, object>();
    }

    public void Add(string key, object value)
    {
        data[key] = value;
    }

    public void Delete(string key)
    {
        data.Remove(key);
    }

    public void Clear()
    {
        data.Clear();
    }

    public object Get(string key)
    {
        return data[key];
    }
}