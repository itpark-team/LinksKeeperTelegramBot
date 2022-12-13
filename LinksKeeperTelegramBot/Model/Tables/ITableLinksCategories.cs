using LinksKeeperTelegramBot.Model.Entities;

namespace LinksKeeperTelegramBot.Model.Tables;

public interface ITableLinksCategories
{
    LinkCategory GetById(int findId);
    bool ContaintByChatId(long chatId);
    void AddNew(LinkCategory linkCategory);
    IEnumerable<LinkCategory> GetAllByChatId(long chatId);
    void DeleteById(int id);
}