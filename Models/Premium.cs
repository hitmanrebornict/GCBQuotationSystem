using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class Premium
{
    public int Id { get; set; }

    public string PremiumName { get; set; } = null!;

    public decimal PremiumCost { get; set; }

    public DateOnly DateFrom { get; set; }

    public DateOnly DateTo { get; set; }
}
