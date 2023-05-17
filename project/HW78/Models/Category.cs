using System;
using System.Collections.Generic;

namespace HW78.Models;

public partial class Category
{
    public int IdCategory { get; set; }

    public string NameCategory { get; set; } = null!;

    public bool IsVisible { get; set; } = true;

    public bool IsActive { get; set; } = true;

    public virtual IEnumerable<Expense> Expenses { get; set; }
}
