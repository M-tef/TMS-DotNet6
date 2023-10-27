using System;
using System.Collections.Generic;

namespace TMSBlazorAPI.Data;

public partial class Refreshtoken
{
    public int UserId { get; set; }

    public string? Email { get; set; }

    public string? TokenId { get; set; }

    public string? Refreshtoken1 { get; set; }

    public bool? IsActive { get; set; }

    public virtual User User { get; set; } = null!;
}
