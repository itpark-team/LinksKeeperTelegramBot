using LinksKeeperTelegramBot.Model.Connection;
using LinksKeeperTelegramBot.Model.Entities;

namespace LinksKeeperTelegramBot.Model.Tables;

public class EfTableLinks : ITableLinks
{
    private LinksKeeperDbContext _dbContext;

    public EfTableLinks(LinksKeeperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddNew(Link link)
    {
        _dbContext.Links.Add(link);
        _dbContext.SaveChanges();
    }

    public IEnumerable<Link> GetAllByCategoryId(int findCategoryId)
    {
        return _dbContext.Links.Where(link => link.CategoryId == findCategoryId).OrderBy(link => link.Id).ToList();
    }

    public IEnumerable<Link> GetAllByCategoryIdWithStartLinkId(int findCategoryId, int startLinkId)
    {
        return _dbContext.Links.Where(link => link.CategoryId == findCategoryId && link.Id >= startLinkId)
            .OrderBy(link => link.Id).ToList();
    }

    public void DeleteById(int id)
    {
        Link link = _dbContext.Links.First(link => link.Id == id);
        _dbContext.Links.Remove(link);
        _dbContext.SaveChanges();
    }
}