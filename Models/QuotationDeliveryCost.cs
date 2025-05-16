using System;
using System.Collections.Generic;

namespace GCBQuotationSystem.Models;

public partial class QuotationDeliveryCost
{
    public int Id { get; set; }

    public int QuotationRecipeId { get; set; }

    public string DeliveryName { get; set; } = null!;

    public decimal CostAmount { get; set; }

    public virtual QuotationRecipe QuotationRecipe { get; set; } = null!;
}
