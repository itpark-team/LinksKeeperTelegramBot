using LinksKeeperTelegramBot.Model.Connection;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Util;

namespace LinksKeeperTelegramBotTest.Model.Tables;

public class TableLinksCategoriesTest
{
    [Fact]
    public void GetById_Id5_ReturnCategoryName_Общее()
    {
        //подготовка
        DbConnector dbConnector = new DbConnector(
            SystemStringsStorage.DbHostTest,
            SystemStringsStorage.DbUsernameTest,
            SystemStringsStorage.DbPasswordTest,
            SystemStringsStorage.DbDatabasenameTest);

        TableLinksCategories tableLinksCategories = new TableLinksCategories(dbConnector.Connection);
        
        //тестирование
        string expectedName = "Общее";
        string actualName = tableLinksCategories.GetById(5).Name;

        //проверка
        Assert.Equal(expectedName, actualName);
    }
}