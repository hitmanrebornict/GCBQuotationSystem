using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class RawMaterialPriceUpdate
{
    public int PriceUpdateId { get; set; }

    public DateOnly PriceMonth { get; set; }

    public DateTime UpdateDate { get; set; }

    public string? Remark { get; set; }

    public virtual ICollection<RawMaterialPriceDetail> RawMaterialPriceDetails { get; set; } = new List<RawMaterialPriceDetail>();
}
