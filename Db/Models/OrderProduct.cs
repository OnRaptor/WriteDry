using System;
using System.Collections.Generic;

namespace WriteDry;

public partial class Orderproduct
{
    public int OrderId { get; set; }

    public string ProductArticleNumber { get; set; }

    public int ProductCount { get; set; }

    public virtual Order Order { get; set; }

    public virtual Product ProductArticleNumberNavigation { get; set; }
}
