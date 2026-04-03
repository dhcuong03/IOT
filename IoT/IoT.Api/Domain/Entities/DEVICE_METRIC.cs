using System;
using System.Collections.Generic;

namespace IoT.Api.Domain.Entities;

public partial class DEVICE_METRIC
{
    public decimal METRIC_ID { get; set; }

    public string METRIC_CODE { get; set; } = null!;

    public string? METRIC_NAME { get; set; }

    public string? UNIT { get; set; }

    public decimal? MIN_VALUE { get; set; }

    public decimal? MAX_VALUE { get; set; }

    public DateTime? CREATED_AT { get; set; }
}
