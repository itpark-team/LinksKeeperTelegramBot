namespace LinksKeeperTelegramBot.Model.Entities;

public class Link
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public long ChatId { get; set; }
}