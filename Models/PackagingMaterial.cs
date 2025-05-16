using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class PackagingMaterial
{
    public int PmId { get; set; }

    public string? Type { get; set; }

    public string? Packaging { get; set; }

    public string? Product { get; set; }

    public string? Weight { get; set; }

    public decimal? Cost100kgEuro { get; set; }

    public decimal? CostPerTon { get; set; }

    public decimal? ExchangeRate { get; set; }

    public decimal? CostGbp100kg { get; set; }

    public decimal? CostGbpton { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
