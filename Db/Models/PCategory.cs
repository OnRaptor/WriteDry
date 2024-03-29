﻿using System;
using System.Collections.Generic;

namespace WriteDry;

public partial class Pcategory
{
    public int Id { get; set; }

    public string CategoryName { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
