using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class QuoteStatus
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}
