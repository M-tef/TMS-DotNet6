using System;
using System.Collections.Generic;

namespace TMSBlazorAPI.Data;

public partial class Admin
{
    public int Id { get; set; }

    public byte? AdminId { get; set; }

    public int? UserId { get; set; }

    public int? AdmintatusId { get; set; }

    public virtual User? User { get; set; }
}
