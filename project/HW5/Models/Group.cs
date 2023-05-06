using System;
using System.Collections.Generic;

namespace HW5.Models;

public partial class Group
{
    public int GrId { get; set; }

    public string GrName { get; set; } = null!;

    public string GrTemp { get; set; } = null!;

    public virtual ICollection<Analysis> Analyses { get; set; } = new List<Analysis>();
}
