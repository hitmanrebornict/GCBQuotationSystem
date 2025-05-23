using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class DeliveryCost
{
    public int DeliveryId { get; set; }

    public int Zone { get; set; }

    public string PostCode { get; set; } = null!;

    public string Pallet { get; set; } = null!;

    public decimal Cost { get; set; }

    public int ServiceHours { get; set; }
}
