using System;
using System.Collections.Generic;

namespace IoT.Api.Domain.Entities;

public partial class IOT_ALERT
{
    public decimal ALERT_ID { get; set; }

    public decimal? DEVICE_ID { get; set; }

    public string? METRIC_CODE { get; set; }

    public string? ALERT_TYPE { get; set; }

    public string? MESSAGE { get; set; }

    public DateTime? CREATED_AT { get; set; }

    public virtual IOT_DEVICE? DEVICE { get; set; }
}
