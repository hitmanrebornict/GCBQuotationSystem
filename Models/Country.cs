using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    public virtual ICollection<CustomerDeliveryDetail> CustomerDeliveryDetails { get; set; } = new List<CustomerDeliveryDetail>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
