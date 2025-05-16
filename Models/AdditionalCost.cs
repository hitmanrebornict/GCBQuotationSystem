using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class AdditionalCost
{
    public int AdditionalCostId { get; set; }

    public string CostName { get; set; } = null!;

    public decimal DefaultAmount { get; set; }
}
