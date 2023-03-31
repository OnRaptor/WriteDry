using System;
using System.Collections.Generic;

namespace WriteDry;

public partial class Point
{
    public int PointId { get; set; }

    public int? Index { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public int? House { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
