using System;
using System.Collections.Generic;

namespace WriteDry;

public partial class Pmanufacturer
{
    public int PmanufacturerId { get; set; }

    public string ProductManufacturer { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
