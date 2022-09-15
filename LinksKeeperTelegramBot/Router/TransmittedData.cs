using LinksKeeperTelegramBot.Model;

namespace LinksKeeperTelegramBot.Router;

public class TransmittedData
{
    public State State { get; }
    public DataStorage DataStorage { get; }

    public TransmittedData()
    {
        State = State.WaitingCommandStart;
        DataStorage = new DataStorage();
    }
}