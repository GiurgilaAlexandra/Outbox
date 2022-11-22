using System;
using System.Collections.Generic;

namespace POC.Outbox.WebAPI.Models.DB;

public partial class Outbox
{
    public long Id { get; set; }

    public byte[] Data { get; set; } = null!;

    public DateTimeOffset WriteTime { get; set; }
}
