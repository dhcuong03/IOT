using System;
using System.Collections.Generic;

namespace IoT.Api.Domain.Entities;

public partial class IOT_DEVICE_CONFIG
{
    public decimal CONFIG_ID { get; set; }

    public decimal DEVICE_ID { get; set; }

    public string? PARAM_NAME { get; set; }

    public string? PARAM_VALUE { get; set; }

    public DateTime? UPDATED_AT { get; set; }

    public virtual IOT_DEVICE DEVICE { get; set; } = null!;
}
