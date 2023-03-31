using System;
using System.Collections.Generic;

namespace WriteDry;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderStatus { get; set; }

    public DateTime OrderDeliveryDate { get; set; }

    public int OrderPickupPoint { get; set; }

    public string OrderFullname { get; set; }

    public int OrderCode { get; set; }

    public DateOnly OrderDate { get; set; }

    public virtual Point OrderPickupPointNavigation { get; set; }
}
