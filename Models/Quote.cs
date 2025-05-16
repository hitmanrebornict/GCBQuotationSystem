using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class Quote
{
    public int QuoteId { get; set; }

    public int CustomerId { get; set; }

    public int DeliveryDetailId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int StatusId { get; set; }

    public int CurrencyId { get; set; }

    public string? Remark { get; set; }

    public string? CustomerRemark { get; set; }

    public virtual Currency Currency { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual CustomerDeliveryDetail DeliveryDetail { get; set; } = null!;

    public virtual ICollection<QuotationRecipe> QuotationRecipes { get; set; } = new List<QuotationRecipe>();

    public virtual QuoteStatus Status { get; set; } = null!;
}
