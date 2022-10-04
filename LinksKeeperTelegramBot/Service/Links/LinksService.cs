using LinksKeeperTelegramBot.Router;
using LinksKeeperTelegramBot.Service.SharedProcessors;
using LinksKeeperTelegramBot.Util.ReplyTextsInitializer;
using LinksKeeperTelegramBot.Util.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LinksKeeperTelegramBot.Service.Links;

public class LinksService
{
    public Task ProcessInputLinkUrlForAdd(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        string requestMessageText = update.Message.Text;
        string responseMessageText = StringsStorage.Empty;

        if (responseMessageText == StringsStorage.CommandReset)
        {
            return GlobalServices.ProcessCommandReset(chatId, transmittedData, botClient, cancellationToken);
        }

        string url = requestMessageText;

        if (!url.StartsWith(StringsStorage.Http) && !url.StartsWith(StringsStorage.Https) ||
            !(url.Length >= 10 && url.Length <= 2048))
        {
            responseMessageText = ReplyTextsStorage.LinkInputUrlErrorInput;

            return botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                cancellationToken: cancellationToken);
        }

        transmittedData.State = State.WaitingInputLinkDescriptionForAdd;
        transmittedData.DataStorage.Add(StringsStorage.DataStorageKeyLinkUrl, url);

        responseMessageText = ReplyTextsStorage.LinkInputUrlSuccessInput;

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            cancellationToken: cancellationToken);
    }

    public Task ProcessInputLinkDescriptionForAdd(long chatId, TransmittedData transmittedData, Update update,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        string requestMessageText = update.Message.Text;
        string responseMessageText = StringsStorage.Empty;

        if (responseMessageText == StringsStorage.CommandReset)
        {
            return GlobalServices.ProcessCommandReset(chatId, transmittedData, botClient, cancellationToken);
        }

        string description = requestMessageText;

        if (!(description.Length >= 3 && description.Length <= 256))
        {
            responseMessageText = ReplyTextsStorage.LinkInputDescriptionErrorInput;

            return botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                cancellationToken: cancellationToken);
        }

        transmittedData.State = State.WaitingClickOnInlineButtonLinkCategoryForAdd;
        transmittedData.DataStorage.Add(StringsStorage.DataStorageKeyLinkDescription, description);

        responseMessageText = ReplyTextsStorage.LinkInputDescriptionSuccessInput;

        return botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            cancellationToken: cancellationToken);
    }
}