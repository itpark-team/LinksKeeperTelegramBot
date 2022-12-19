using LinksKeeperTelegramBot.Util;
using Telegram.Bot.Types.ReplyMarkups;

namespace LinksKeeperTelegramBot.BotInitializer;

public class BotTextMessage
{
    public string Text { get; }
    public InlineKeyboardMarkup InlineKeyboardMarkup { get; }

    public BotTextMessage(string text)
    {
        Text = text;
        InlineKeyboardMarkup = null;
    }

    public BotTextMessage(string text, InlineKeyboardMarkup inlineKeyboardMarkup)
    {
        Text = text;
        InlineKeyboardMarkup = inlineKeyboardMarkup;
    }
}