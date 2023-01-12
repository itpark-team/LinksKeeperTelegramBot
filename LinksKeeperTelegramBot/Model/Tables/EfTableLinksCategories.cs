using LinksKeeperTelegramBot.Model.Connection;
using LinksKeeperTelegramBot.Model.Entities;

namespace LinksKeeperTelegramBot.Model.Tables;

public class EfTableLinksCategories : ITableLinksCategories
{
    private LinksKeeperDbContext _dbContext;

    public EfTableLinksCategories(LinksKeeperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public LinkCategory GetById(int findId)
    {
        return _dbContext.LinksCategories.First(linkCategory => linkCategory.Id == findId);
    }

    public bool ContaintByChatId(long chatId)
    {
        return _dbContext.LinksCategories.FirstOrDefault(linkCategory => linkCategory.ChatId == chatId) != null;
    }

    public void AddNew(LinkCategory linkCategory)
    {
        _dbContext.LinksCategories.Add(linkCategory);
        _dbContext.SaveChanges();
    }

    public IEnumerable<LinkCategory> GetAllByChatId(long chatId)
    {
        return _dbContext.LinksCategories.Where(linkCategory => linkCategory.ChatId == chatId)
            .OrderBy(linkCategory => linkCategory.Id).ToList();
    }

    public void DeleteById(int id)
    {
        LinkCategory linkCategory = _dbContext.LinksCategories.First(linkCategory => linkCategory.Id == id);
        _dbContext.LinksCategories.Remove(linkCategory);
        _dbContext.SaveChanges();
    }
}