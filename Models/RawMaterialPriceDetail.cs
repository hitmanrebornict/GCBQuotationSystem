using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class RawMaterialPriceDetail
{
    public int PriceDetailId { get; set; }

    public int PriceUpdateId { get; set; }

    public int MaterialId { get; set; }

    public decimal Price { get; set; }

    public virtual RawMaterial Material { get; set; } = null!;

    public virtual RawMaterialPriceUpdate PriceUpdate { get; set; } = null!;
}
