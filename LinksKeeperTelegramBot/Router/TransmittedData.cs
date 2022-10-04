using LinksKeeperTelegramBot.Model;

namespace LinksKeeperTelegramBot.Router;

public class TransmittedData
{
    public State State { get; set; }
    public DataStorage DataStorage { get; private set; }

    public TransmittedData()
    {
        State = State.WaitingCommandStart;
        DataStorage = new DataStorage();
    }

    public void ClearDataStorage()
    {
        DataStorage = new DataStorage();
    }
}