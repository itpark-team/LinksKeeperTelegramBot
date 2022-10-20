using LinksKeeperTelegramBot.BotSettings;
using LinksKeeperTelegramBot.Model;
using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Util;
using NLog;

namespace LinksKeeperTelegramBot.Service.Handlers;

public class MainMenuService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    public BotTextMessage ProcessCommandStart(string command, TransmittedData transmittedData)
    {
        if (command != SystemStringsStorage.CommandStart)
        {
            return new BotTextMessage(DialogsStringsStorage.CommandStartInputErrorInput);
        }

        transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;

        return new BotTextMessage(
            DialogsStringsStorage.MenuMain,
            InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain
        );
    }

    public BotTextMessage ProcessClickOnInlineButtonInMenuMain(string callBackData, TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonAddInMenuMain.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuAdd;

            return new BotTextMessage(
                DialogsStringsStorage.MenuAdd,
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuAdd
            );
        }
        else if (callBackData == BotButtonsStorage.ButtonShowInMenuMain.CallBackData)
        {
            if (DbManager.GetInstance().TableLinksCategories.ContaintByChatId(transmittedData.ChatId) == false)
            {
                return new BotTextMessage(DialogsStringsStorage.MenuShowNoCategories);
            }

            transmittedData.State = State.WaitingClickOnInlineButtonLinkCategoryForShow;

            return new BotTextMessage(
                DialogsStringsStorage.MenuShow,
                InlineKeyboardsMarkupStorage.CreateInlineKeyboardMarkupMenuLinkCategoryForShow(transmittedData.ChatId)
            );
        }
        else if (callBackData == BotButtonsStorage.ButtonEditInMenuMain.CallBackData)
        {
            return new BotTextMessage("Нажата клавиша Редактировать");
        }
        else if (callBackData == BotButtonsStorage.ButtonDeleteInMenuMain.CallBackData)
        {
            return new BotTextMessage("Нажата клавиша Удалить");
        }
        else if (callBackData == BotButtonsStorage.ButtonHowToUseInMenuMain.CallBackData)
        {
            return new BotTextMessage("Нажата клавиша Как пользоваться");
        }

        return new BotTextMessage();
    }

    public BotTextMessage ProcessClickOnInlineButtonInMenuAddChoosing(string callBackData, TransmittedData transmittedData)
    {
        if (callBackData == BotButtonsStorage.ButtonLinkInMenuAdd.CallBackData)
        {
            transmittedData.State = State.WaitingInputLinkUrlForAdd;

            return new BotTextMessage(DialogsStringsStorage.LinkInputUrl);
        }
        else if (callBackData == BotButtonsStorage.ButtonCategoryInMenuAdd.CallBackData)
        {
            return new BotTextMessage();
        }
        else if (callBackData == BotButtonsStorage.ButtonBackwardInMenuAdd.CallBackData)
        {
            transmittedData.State = State.WaitingClickOnInlineButtonInMenuMain;

            return new BotTextMessage(
                DialogsStringsStorage.MenuMain,
                InlineKeyboardsMarkupStorage.InlineKeyboardMarkupMenuMain
            );
        }

        return new BotTextMessage();
    }
}