using System;
using System.Collections.Generic;

namespace LinksKeeperTelegramBot.Model.Entities;

public partial class Link
{
    public int Id { get; set; }

    public string Url { get; set; }

    public string Description { get; set; }

    public int CategoryId { get; set; }

    public long ChatId { get; set; }

    public virtual LinkCategory Category { get; set; }
}
