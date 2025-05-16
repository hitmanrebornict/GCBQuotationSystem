using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class PremiumCost
{
    public int PremiumCostId { get; set; }

    public string? Name { get; set; }

    public decimal? Amount { get; set; }
}
