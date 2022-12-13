using LinksKeeperTelegramBot.BotInitializer;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Model.Tables;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Service.MenuPoints;
using LinksKeeperTelegramBot.Util;
using Moq;

namespace LinksKeeperTelegramBotTest;

public class MainMenuServiceTest
{
    [Fact]
    public void ProcessCommandStart_WrongCommand_ReturnTextCommandStartInputErrorInput()
    {
        //подготовка
        string command = "WrongCommand";

        MainMenuService mainMenuService = new MainMenuService(null);

        //тестирование
        BotTextMessage botTextMessage = mainMenuService.ProcessCommandStart(command, null);

        string expectedText = DialogsStringsStorage.CommandStartInputErrorInput;
        string actualText = botTextMessage.Text;

        //проверка
        Assert.Equal(expectedText, actualText);
    }

    [Fact]
    public void ProcessClickInMenuMain_WrongCallBackData_ReturnException()
    {
        //подготовка
        string callBackData = "WrongCallBackData";

        MainMenuService mainMenuService = new MainMenuService(null);

        //тестирование
        //проверка

        Assert.Throws<Exception>(() => mainMenuService.ProcessClickInMenuMain(callBackData, null));
    }

    [Fact]
    public void ProcessClickInMenuMain_NoLinksCategories_ReturnTextMenuShowNoCategories()
    {
        //подготовка
        string callBackData = BotButtonsStorage.ShowInMenuMain.CallBackData;
        TransmittedData transmittedData = new TransmittedData(0);

        Mock<ITableLinksCategories> tableLinksCategories = new Mock<ITableLinksCategories>();
        tableLinksCategories.Setup(exp => exp.ContaintByChatId(0)).Returns(false);

        DbManager dbManager = new DbManager(null, tableLinksCategories.Object);

        MainMenuService mainMenuService = new MainMenuService(dbManager);

        //тестирование
        BotTextMessage botTextMessage = mainMenuService.ProcessClickInMenuMain(callBackData, transmittedData);

        string expectedText = DialogsStringsStorage.MenuShowNoCategories;
        string actualText = botTextMessage.Text;

        //проверка
        Assert.Equal(expectedText, actualText);
    }
}