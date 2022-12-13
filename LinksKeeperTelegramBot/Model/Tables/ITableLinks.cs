using LinksKeeperTelegramBot.Model.Entities;

namespace LinksKeeperTelegramBot.Model.Tables;

public interface ITableLinks
{
    void AddNew(Link link);
    IEnumerable<Link> GetAllByCategoryId(int findCategoryId);
    IEnumerable<Link> GetAllByCategoryIdWithStartLinkId(int findCategoryId, int startLinkId);
    void DeleteById(int id);
}