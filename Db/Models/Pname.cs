using System;
using System.Collections.Generic;

namespace WriteDry;

public partial class Pname
{
    public int PnameId { get; set; }

    public string ProductName { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
