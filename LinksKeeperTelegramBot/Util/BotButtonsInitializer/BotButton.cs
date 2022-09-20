namespace LinksKeeperTelegramBot.Util.BotButtonsInitializer;

public class BotButton
{
    public string Name { get; }
    public string CallBackData { get; }

    public BotButton(string name, string callBackData)
    {
        Name = name;
        CallBackData = callBackData;
    }
}