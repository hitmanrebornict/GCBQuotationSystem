using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class Currency
{
    public int CurrencyId { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}
