using System;
using System.Collections.Generic;

namespace HW78.Models;

public partial class Expense
{
    public int IdExpense { get; set; }

    public double CostExpense { get; set; }

    public string? Commentary { get; set; } = null!;

    public int FkCategory { get; set; }

    public bool? IsVisible { get; set; }
    public DateTime DateTime { get; set; }

    public virtual Category FkCategoryNavigation { get; set; } = null!;
}
