using System;
using System.Collections.Generic;

namespace TMSBlazorAPI.Data;

public partial class AdminStatus
{
    public byte StatusId { get; set; }

    public string? AdminStatus1 { get; set; }

    public virtual AdminsDescription Status { get; set; } = null!;
}
