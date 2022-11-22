using System;
using System.Collections.Generic;

namespace POC.Outbox.WebAPI.Models.DB;

public partial class Order
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTimeOffset? WriteTime { get; set; }
}
