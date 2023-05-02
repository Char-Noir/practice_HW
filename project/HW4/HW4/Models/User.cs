using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HW4.Models;

public partial class User
{
    public int UserId { get; set; }

    [Required]
    public string UserName { get; set; } = null!;
    
    [EmailAddress]
    [Required]

    public string UserEmail { get; set; } = null!;

    public int UserRole { get; set; }

    public virtual Role UserRoleNavigation { get; set; } = null!;
}
