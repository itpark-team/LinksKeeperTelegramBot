using LinksKeeperTelegramBot.Util;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.BotSettings;

public class BotTextMessage
{
    private string Text { get; }
    private InlineKeyboardMarkup InlineKeyboardMarkup { get; }
    
    public BotTextMessage()
    {
        Text = SystemStringsStorage.Empty;
        InlineKeyboardMarkup = InlineKeyboardMarkup.Empty();
    }
    
    public BotTextMessage(string text)
    {
        Text = text;
        InlineKeyboardMarkup = InlineKeyboardMarkup.Empty();
    }
    
    public BotTextMessage(string text, InlineKeyboardMarkup inlineKeyboardMarkup)
    {
        Text = text;
        InlineKeyboardMarkup = inlineKeyboardMarkup;
    }
}