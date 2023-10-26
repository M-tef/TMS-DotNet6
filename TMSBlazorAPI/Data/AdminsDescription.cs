using System;
using System.Collections.Generic;

namespace TMSBlazorAPI.Data;

public partial class AdminsDescription
{
    public byte AdminId { get; set; }

    public bool? AdminStatusId { get; set; }

    public string? AdminDescription { get; set; }

    public virtual AdminStatus? AdminStatus { get; set; }
}
