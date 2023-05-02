using System;
using System.Collections.Generic;

namespace HW4.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public int UserRole { get; set; }

    public virtual Role UserRoleNavigation { get; set; } = null!;
}
