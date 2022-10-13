namespace LinksKeeperTelegramBot.Util.ReplyTextsInitializer;

public class ReplyTextsStorage
{
    public static string CommandStartInputErrorInput =
        "Команда не распознана. Для начала работы с ботом введите /start";

    public static string MenuMain = "Доброго времени друзья\nВы в главном меню\nВыберите действие";
    public static string MenuAdd = "Выерите, что вы хотите добавить?";
    public static string LinkInputUrl = "Введите URL";

    public static string LinkInputUrlError =
        "URL не распознан. Пожалуйста введите корректный URL от 10 до 2048 символов";
    public static string LinkInputUrlSuccess =
        "URL успешно сохранён.\nВведите описание ссылки:";
    
    public static string LinkInputDescriptionError =
        "Ошибка ввода описания. Пожалуйста введите корректное описание URL от 3 до 256 символов";
    public static string LinkInputDescriptionSuccess =
        "Описание успешно сохранено.\nВыберите категорию ссылки:";

    public static string CreateLinkInputCategory(string url, string description, string categoryName)
    {
        return $"Ссылка: {url}\n\n" +
               $"Описание: {description}\n\n" +
               $"Категория: {categoryName}\n\n" +
               $"Сохранить ссылку?";
    }
    
    public static string LinkApproveAddYes = "Ваша ссылка успешно сохранена\nВыберите дальнейшее действие.";
    public static string LinkApproveAddNo = "Сохранение вашей ссылки успешно отменено\nВыберите дальнейшее действие.";
}