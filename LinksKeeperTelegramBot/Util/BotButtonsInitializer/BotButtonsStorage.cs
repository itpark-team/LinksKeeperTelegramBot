namespace LinksKeeperTelegramBot.Util.BotButtonsInitializer;

public class BotButtonsStorage
{
    public static BotButton ButtonAddInMainMenu { get; } = new BotButton("Добавить", "addInMainMenu");
    public static BotButton ButtonShowInMainMenu { get; } = new BotButton("Просмотреть", "showInMainMenu");
    public static BotButton ButtonEditInMainMenu { get; } = new BotButton("Редактировать", "editInMainMenu");
    public static BotButton ButtonDeleteInMainMenu { get; } = new BotButton("Удалить", "deleteInMainMenu");
    public static BotButton ButtonHowToUseInMainMenu { get; } = new BotButton("Как пользоваться", "howToUseInMainMenu");
}