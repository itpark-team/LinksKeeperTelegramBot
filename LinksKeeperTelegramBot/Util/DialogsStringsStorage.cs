namespace LinksKeeperTelegramBot.Util.ReplyTextsInitializer;

public class DialogsStringsStorage
{
    public const string CommandStartInputErrorInput =
        "Команда не распознана. Для начала работы с ботом введите /start";

    public const string MenuMain = "Доброго времени друзья\nВы в главном меню\nВыберите действие";
    public const string MenuAdd = "Выерите, что вы хотите добавить?";
    public const string LinkInputUrl = "Введите URL";

    public const string LinkInputUrlError =
        "URL не распознан. Пожалуйста введите корректный URL от 10 до 2048 символов";
    public const string LinkInputUrlSuccess =
        "URL успешно сохранён.\nВведите описание ссылки:";
    
    public const string LinkInputDescriptionError =
        "Ошибка ввода описания. Пожалуйста введите корректное описание URL от 3 до 256 символов";
    public const string LinkInputDescriptionSuccess =
        "Описание успешно сохранено.\nВыберите категорию ссылки:";

    public static string CreateLinkInputCategory(string url, string description, string categoryName)
    {
        return $"Ссылка: {url}\n\n" +
               $"Описание: {description}\n\n" +
               $"Категория: {categoryName}\n\n" +
               $"Сохранить ссылку?";
    }
    
    public const string LinkApproveAddYes = "Ваша ссылка успешно сохранена\nВыберите дальнейшее действие.";
    public const string LinkApproveAddNo = "Сохранение вашей ссылки успешно отменено\nВыберите дальнейшее действие.";
    
    public const string MenuShow = "Выберите, категорию, ссылки в которой вы хотите просмотреть";
    public const string MenuShowNoCategories = "У вас нет ещё ни одной добавленной ссылки, пожалуйста вначале добавьте ссылку";
}