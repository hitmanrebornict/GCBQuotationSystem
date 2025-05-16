using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class CustomerDeliveryDetail
{
    public int DeliveryId { get; set; }

    public int CustNo { get; set; }

    public string DeliveryName { get; set; } = null!;

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public string? Postcode { get; set; }

    public int CountryId { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual Customer CustNoNavigation { get; set; } = null!;

    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}
