using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class Customer
{
    public int CustNo { get; set; }

    public string CustName { get; set; } = null!;

    public string? InvoiceAddress1 { get; set; }

    public string? InvoiceAddress2 { get; set; }

    public string? InvoiceCity { get; set; }

    public string? InvoicePostcode { get; set; }

    public int? CountryId { get; set; }

    public bool Active { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<CustomerDeliveryDetail> CustomerDeliveryDetails { get; set; } = new List<CustomerDeliveryDetail>();

    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}
