using System;
using System.Collections.Generic;

namespace TMSBlazorAPI.Data;

public partial class Club
{
    public int ClubId { get; set; }

    public string ClubName { get; set; } = null!;

    public int? UserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User? User { get; set; }
}
