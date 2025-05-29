using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class ProductionCost
{
    public int Id { get; set; }

    public string ProductType { get; set; } = null!;

    public decimal ProductTypeCost { get; set; }

    public bool Active { get; set; }
}
