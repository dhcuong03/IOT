using System;
using System.Collections.Generic;

namespace IoT.Api.Domain.Entities;

public partial class IOT_DEVICE
{
    public decimal DEVICE_ID { get; set; }

    public string DEVICE_CODE { get; set; } = null!;

    public decimal MACHINE_ID { get; set; }

    public string? PROTOCOL { get; set; }

    public string? IP_ADDRESS { get; set; }

    public decimal? PORT { get; set; }

    public string? STATUS { get; set; }

    public DateTime? LAST_CONNECTED { get; set; }

    public DateTime? CREATED_AT { get; set; }

    public virtual ICollection<IOT_ALERT> IOT_ALERTs { get; set; } = new List<IOT_ALERT>();

    public virtual ICollection<IOT_CONNECTION_LOG> IOT_CONNECTION_LOGs { get; set; } = new List<IOT_CONNECTION_LOG>();

    public virtual ICollection<IOT_DEVICE_CONFIG> IOT_DEVICE_CONFIGs { get; set; } = new List<IOT_DEVICE_CONFIG>();

    public virtual ICollection<IOT_DEVICE_DATum> IOT_DEVICE_DATa { get; set; } = new List<IOT_DEVICE_DATum>();

    public virtual MACHINE MACHINE { get; set; } = null!;
}
