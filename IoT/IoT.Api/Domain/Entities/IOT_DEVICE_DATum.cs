using System;
using System.Collections.Generic;

namespace IoT.Api.Domain.Entities;

public partial class IOT_DEVICE_DATum
{
    public decimal DATA_ID { get; set; }

    public decimal DEVICE_ID { get; set; }

    public string? DATA_JSON { get; set; }

    public DateTime? RECORDED_AT { get; set; }

    public virtual IOT_DEVICE DEVICE { get; set; } = null!;
}
