using LinksKeeperTelegramBot.BotInitializer;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Service.MenuPoints;
using LinksKeeperTelegramBot.Util;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBotTest;

public class SharedServicesTest
{
    [Fact]
    public void GotoProcessClickInMenuMain_ReturnStateTextKeyboardMenuMain()
    {
        //подготовка 
        TransmittedData transmittedData = new TransmittedData(0);
        transmittedData.DataStorage.AddOrUpdate("key", "value");

        //тестирование
        BotTextMessage botTextMessage = SharedService.GotoProcessClickInMenuMain(transmittedData);

        string expectedText = DialogsStringsStorage.MenuMain;
        string actualText = botTextMessage.Text;

        InlineKeyboardMarkup expectedKeyboard = InlineKeyboardsMarkupStorage.MenuMain;
        InlineKeyboardMarkup actualKeyboard = botTextMessage.InlineKeyboardMarkup;

        State expectedState = State.ClickInMenuMain;
        State actualState = transmittedData.State;

        int expectedDataStorageCount = 0;
        int actualDataStorageCount = transmittedData.DataStorage.Count;

        //проверка
        Assert.Equal(expectedText, actualText);

        Assert.Equal(expectedKeyboard, actualKeyboard);

        Assert.Equal(expectedState, actualState);

        Assert.Equal(expectedDataStorageCount, actualDataStorageCount);
    }
}