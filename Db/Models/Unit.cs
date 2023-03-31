using System;
using System.Collections.Generic;

namespace WriteDry;

public partial class Unit
{
    public int Id { get; set; }

    public string UnitName { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
