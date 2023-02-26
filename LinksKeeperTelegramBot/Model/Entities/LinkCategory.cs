using System;
using System.Collections.Generic;

namespace LinksKeeperTelegramBot.Model.Entities;

public partial class LinkCategory
{
    public int Id { get; set; }

    public string Name { get; set; }

    public long ChatId { get; set; }

    public virtual ICollection<Link> Links { get; } = new List<Link>();
}
