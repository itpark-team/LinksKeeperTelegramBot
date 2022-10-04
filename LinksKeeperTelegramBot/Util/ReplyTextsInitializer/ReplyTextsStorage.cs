namespace LinksKeeperTelegramBot.Util.ReplyTextsInitializer;

public class ReplyTextsStorage
{
    public static string CommandStartInputErrorInput =
        "Команда не распознана. Для начала работы с ботом введите /start";

    public static string MenuMain = "Доброго времени друзья\nВы в главном меню\nВыберите действие";
    public static string MenuAdd = "Выерите, что вы хотите добавить?";
    public static string LinkInputUrl = "Введите URL";

    public static string LinkInputUrlErrorInput =
        "URL не распознан. Пожалуйста введите корректный URL от 10 до 2048 символов";
    public static string LinkInputUrlSuccessInput =
            "URL успешно сохранён.\nВведите описание ссылки:";
    
    public static string LinkInputDescriptionErrorInput =
        "Ошибка ввода описания. Пожалуйста введите корректное описание URL от 3 до 256 символов";
    public static string LinkInputDescriptionSuccessInput =
        "Описание успешно сохранено.\nВыберите категорию ссылки:";
}