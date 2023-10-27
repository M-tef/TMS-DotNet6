using System;
using System.Collections.Generic;

namespace TMSBlazorAPI.Data;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? MembershipStartDate { get; set; }

    public string Username { get; set; } = null!;

    public string? Password { get; set; }

    public string? Role { get; set; }

    public string? MembershipStatus { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();

    public virtual Refreshtoken? Refreshtoken { get; set; }
}
