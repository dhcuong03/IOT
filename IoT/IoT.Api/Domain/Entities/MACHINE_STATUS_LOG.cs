using System;
using System.Collections.Generic;

namespace IoT.Api.Domain.Entities;

public partial class MACHINE_STATUS_LOG
{
    public decimal LOG_ID { get; set; }

    public decimal? MACHINE_ID { get; set; }

    public string? STATUS { get; set; }

    public string? MESSAGE { get; set; }

    public DateTime? CREATED_AT { get; set; }

    public virtual MACHINE? MACHINE { get; set; }
}
