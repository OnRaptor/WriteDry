using System;
using System.Collections.Generic;

namespace WriteDry;

public partial class Provider
{
    public int Id { get; set; }

    public string ProviderName { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
