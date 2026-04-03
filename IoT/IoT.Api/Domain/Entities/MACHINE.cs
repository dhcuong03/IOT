using System;
using System.Collections.Generic;

namespace IoT.Api.Domain.Entities;

public partial class MACHINE
{
    public decimal MACHINE_ID { get; set; }

    public string MACHINE_CODE { get; set; } = null!;

    public string? MACHINE_NAME { get; set; }

    public string? MACHINE_TYPE { get; set; }

    public string? LOCATION { get; set; }

    public string? STATUS { get; set; }

    public DateTime? CREATED_AT { get; set; }

    public virtual ICollection<IOT_DEVICE> IOT_DEVICEs { get; set; } = new List<IOT_DEVICE>();

    public virtual ICollection<MACHINE_STATUS_LOG> MACHINE_STATUS_LOGs { get; set; } = new List<MACHINE_STATUS_LOG>();
}
